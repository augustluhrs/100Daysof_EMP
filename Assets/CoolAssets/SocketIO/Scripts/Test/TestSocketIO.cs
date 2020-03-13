#region License
/*
 * TestSocketIO.cs
 *
 * The MIT License
 *
 * Copyright (c) 2014 Fabio Panettieri
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 */
#endregion

using System.Collections;
using UnityEngine;
using SocketIO;

public class TestSocketIO : MonoBehaviour
{
	private SocketIOComponent socket;

	[SerializeField] GameObject prefab;
	[SerializeField] GameObject paint;
	[SerializeField] GameObject x;
	[SerializeField] GameObject o;
	[SerializeField] GameObject space0;
	[SerializeField] GameObject space1;
	[SerializeField] GameObject space2;
	[SerializeField] GameObject space3;
	[SerializeField] GameObject space4;
	[SerializeField] GameObject space5;
	[SerializeField] GameObject space6;
	[SerializeField] GameObject space7;
	[SerializeField] GameObject space8;



	public void Start() 
	{
		GameObject go = GameObject.Find("SocketIO");
		socket = go.GetComponent<SocketIOComponent>();

		socket.On("open", TestOpen);
		socket.On("boop", TestBoop);
		socket.On("error", TestError);
		socket.On("close", TestClose);
		socket.On("clientMsg", ClientTest);
		socket.On("drawing", Draw);
		socket.On("x move", XMove);
		socket.On("o move", OMove);

		
		StartCoroutine("BeepBoop");
	}

	private IEnumerator BeepBoop()
	{
		// wait 1 seconds and continue
		yield return new WaitForSeconds(1);
		
		socket.Emit("beep");
		
		// wait 3 seconds and continue
		yield return new WaitForSeconds(3);
		
		socket.Emit("beep");
		
		// wait 2 seconds and continue
		yield return new WaitForSeconds(2);
		
		socket.Emit("beep");
		
		// wait ONE FRAME and continue
		yield return null;
		
		socket.Emit("beep");
		socket.Emit("beep");
	}
	public void XMove(SocketIOEvent e)
	{
		Debug.Log("x move: " + e.data);
		switch (e.data.GetField("space").n)
		{
			case 0:
				Debug.Log(space0.transform.position);
				Instantiate(x, space0.transform.position + new Vector3(0, 5f, 0), Quaternion.identity);
				break;
			case 1:
				Debug.Log(space1.transform.position);
				Instantiate(x, space1.transform.position + new Vector3(0, 5f, 0), Quaternion.identity);
				break;
			case 2:
				Debug.Log(space2.transform.position);
				Instantiate(x, space2.transform.position + new Vector3(0, 5f, 0), Quaternion.identity);
				break;
			case 3:
				Debug.Log(space3.transform.position);
				Instantiate(x, space3.transform.position + new Vector3(0, 5f, 0), Quaternion.identity);
				break;
			case 4:
				Debug.Log(space4.transform.position);
				Instantiate(x, space4.transform.position + new Vector3(0, 5f, 0), Quaternion.identity);
				break;
			case 5:
				Debug.Log(space5.transform.position);
				Instantiate(x, space5.transform.position + new Vector3(0, 5f, 0), Quaternion.identity);
				break;
			case 6:
				Debug.Log(space6.transform.position);
				Instantiate(x, space6.transform.position + new Vector3(0, 5f, 0), Quaternion.identity);
				break;
			case 7:
				Debug.Log(space7.transform.position);
				Instantiate(x, space7.transform.position + new Vector3(0, 5f, 0), Quaternion.identity);
				break;
			case 8:
				Debug.Log(space8.transform.position);
				Instantiate(x, space8.transform.position + new Vector3(0, 5f, 0), Quaternion.identity);
				break;							
			default:
				Debug.Log("what happened?");
				break;
		}
	}
	public void OMove(SocketIOEvent e)
	{
		Debug.Log("o move: " + e.data);
		switch (e.data.GetField("space").n)
		{
			case 0:
				Debug.Log(space0.transform.position);
				Instantiate(o, space0.transform.position + new Vector3(0, 5f, 0), Quaternion.identity);
				break;
			case 1:
				Debug.Log(space1.transform.position);
				Instantiate(o, space1.transform.position + new Vector3(0, 5f, 0), Quaternion.identity);
				break;
			case 2:
				Debug.Log(space2.transform.position);
				Instantiate(o, space2.transform.position + new Vector3(0, 5f, 0), Quaternion.identity);
				break;
			case 3:
				Debug.Log(space3.transform.position);
				Instantiate(o, space3.transform.position + new Vector3(0, 5f, 0), Quaternion.identity);
				break;
			case 4:
				Debug.Log(space4.transform.position);
				Instantiate(o, space4.transform.position + new Vector3(0, 5f, 0), Quaternion.identity);
				break;
			case 5:
				Debug.Log(space5.transform.position);
				Instantiate(o, space5.transform.position + new Vector3(0, 5f, 0), Quaternion.identity);
				break;
			case 6:
				Debug.Log(space6.transform.position);
				Instantiate(o, space6.transform.position + new Vector3(0, 5f, 0), Quaternion.identity);
				break;
			case 7:
				Debug.Log(space7.transform.position);
				Instantiate(o, space7.transform.position + new Vector3(0, 5f, 0), Quaternion.identity);
				break;
			case 8:
				Debug.Log(space8.transform.position);
				Instantiate(o, space8.transform.position + new Vector3(0, 5f, 0), Quaternion.identity);
				break;							
			default:
				Debug.Log("what happened?");
				break;
		}
	}
	public void Draw(SocketIOEvent e)
	{
		Debug.Log("drawing X Y received: " + e.data); //screen ends up being around 400x750
		// Debug.Log(string.Format("[name: {0}, data: {1}]", e.name, e.data));
		Debug.Log("test: " + e.data.GetField("mX"));
		Instantiate(paint, new Vector3(e.data.GetField("mX").f / 10f, 5f, e.data.GetField("mY").f / -10f), Quaternion.identity);
	}
	public void ClientTest(SocketIOEvent e)
	{
		Debug.Log("client input received");
		Instantiate(prefab, new Vector3(0,5f,0), Quaternion.identity);
	}
	public void TestOpen(SocketIOEvent e)
	{
		Debug.Log("[SocketIO] Open received: " + e.name + " " + e.data);
	}
	
	public void TestBoop(SocketIOEvent e)
	{
		Debug.Log("[SocketIO] Boop received: " + e.name + " " + e.data);

		if (e.data == null) { return; }

		Debug.Log(
			"#####################################################" +
			"THIS: " + e.data.GetField("this").str +
			"#####################################################"
		);
	}
	
	public void TestError(SocketIOEvent e)
	{
		Debug.Log("[SocketIO] Error received: " + e.name + " " + e.data);
	}
	
	public void TestClose(SocketIOEvent e)
	{	
		Debug.Log("[SocketIO] Close received: " + e.name + " " + e.data);
	}
}

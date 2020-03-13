using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;

public class PlatformMove : MonoBehaviour
{
    private SocketIOComponent socket;

    // [SerializeField] GameObject ball;
    [SerializeField] GameObject platform;
    [Range(0, 1)] [SerializeField] float slerpParameter;

    private Transform oldQuat;
    private Transform newQuat;

    // [Range(1,20f)][SerializeField] float chillOut;

    //player data from socket and for quaternions
    public class Player
    {
        public string id;
        public float r;
        public float g;
        public float b;
        public float count;
        public Quaternion thisQuat;
    }

    public List<Player> players = new List<Player>(4); //so 4 max players for now?
                                                       // public pList<Quaternion> QList = new List<Quaternion>(16);

    //UI indicator stuff
    [SerializeField] GameObject can;
    [SerializeField] GameObject phone;
    [SerializeField] GameObject newball;
    // [SerializeField] GameObject ball;


    public void Start()
    {
        
		//socket stuff
        GameObject go = GameObject.Find("SocketIO");
        socket = go.GetComponent<SocketIOComponent>();

        socket.On("open", TestOpen);
        socket.On("boop", TestBoop);
        socket.On("error", TestError);
        socket.On("close", TestClose);

        socket.On("ballMove", MovePlatform);
        socket.On("spawn", SpawnBall);
        socket.On("reading", QuatMovePlatform);
        socket.On("clientDisconnect", DisconnectClient);
        // players = new List<Player>(16); //so 16 max players for now?

        StartCoroutine("BeepBoop");

        oldQuat = platform.transform;
        newQuat = platform.transform;

		
    }
    private void Update()
    {
        //multiple client averaging?
        Quaternion q_average = Quaternion.identity;
        float weight = 1f / players.Count;
        foreach (Player player in players)
        {
            // QList.Add(player.thisQuat);
            //thanks to user Antypodish on https://forum.unity.com/threads/average-quaternions.86898/
            q_average *= Quaternion.Lerp(Quaternion.identity, player.thisQuat, weight);
        }
        platform.transform.rotation = Quaternion.Lerp(oldQuat.rotation, q_average, slerpParameter);


        //old way
        // platform.transform.rotation = Quaternion.Slerp(oldQuat.rotation, newQuat.rotation, slerpParameter);
        oldQuat.rotation = platform.transform.rotation;
        // Debug.Log(oldQuat.rotation.eulerAngles);
    }
    public void SpawnBall(SocketIOEvent e)
    {
        Instantiate(newball, new Vector3(7.6f, 15.5f, -2.8f), Quaternion.identity);
    }
    public void QuatMovePlatform(SocketIOEvent e)
    {
        //check the players list for that player
        Player check = players.Find(x => x.id == e.data.GetField("id").str);
        if (check == null) //new player
        {
            Debug.Log("new player: " + e.data.GetField("id").str);
            //make new player for list
            Player newPlayer = new Player();
            newPlayer.id = e.data.GetField("id").str;
            // newPlayer.r = e.data.GetField("r").f;
            // newPlayer.g = e.data.GetField("g").f;
            // newPlayer.b = e.data.GetField("b").f;
            newPlayer.count = players.Count;
            newPlayer.thisQuat = new Quaternion(e.data.GetField("qX").f * -1f, e.data.GetField("qZ").f * -1f, e.data.GetField("qY").f * -1f, e.data.GetField("qW").f);
            //add player to Players list
            players.Add(newPlayer);
            Debug.Log("Players count: " + players.Count);
            // Quaternion cameraDir = GameObject.Find("CameraRig").GetComponent<CameraRig>().staticDirection;
            // cameraDir = newPlayer.thisQuat; //they'll have to start pointed at the screen
            GameObject.Find("CameraRig").GetComponent<CameraRig>().staticDirection = newPlayer.thisQuat;
            // Debug.Log(GameObject.Find("CameraRig").GetComponent<CameraRig>().staticDirection.eulerAngles);
            //make new UI indicator
            //thanks to user Dibbie at https://answers.unity.com/questions/1165465/how-to-instantiate-ui-elements-centered-around-an.html
            /*Vector3 canPos = can.transform.position; //middle pos
			float spot = players.Count;
			Vector3 pos = new Vector3(canPos.x * (spot*0.4f), canPos.y * 1.5f, canPos.z);
			GameObject newPhone = Instantiate(phone, pos, Quaternion.identity);
			newPhone.name = spot.ToString();
        	newPhone.GetComponent<Renderer>().material.color = new Color(e.data.GetField("r").f / 255f, e.data.GetField("b").f / 255f, e.data.GetField("g").f / 255f, 205f);
			newPhone.transform.SetParent(can.transform, false);
			RectTransform rt = newPhone.GetComponent<RectTransform>(); 
			rt.anchoredPosition.Set(pos.x, pos.y);
			*/
        }
        else //update that player's quaternion
        {
            //gotta flip again... its accurate, just not how i imagine the movement
            //Z movement is around Y axis, Y movement is around Z axis, x is same though
            check.thisQuat = new Quaternion(e.data.GetField("qX").f * -1f, e.data.GetField("qZ").f * -1f, e.data.GetField("qY").f * -1f, e.data.GetField("qW").f);
        }
    }
    public void MovePlatform(SocketIOEvent e)
    {

        //check the players list for that player
        Player check = players.Find(x => x.id == e.data.GetField("id").str);
        if (check == null) //new player
        {
            Debug.Log("new player: " + e.data.GetField("id").str);
            //make new player for list
            Player newPlayer = new Player();
            newPlayer.id = e.data.GetField("id").str;
            newPlayer.r = e.data.GetField("r").f;
            newPlayer.g = e.data.GetField("g").f;
            newPlayer.b = e.data.GetField("b").f;
            newPlayer.count = players.Count;
            newPlayer.thisQuat.eulerAngles = new Vector3(e.data.GetField("ax").f, e.data.GetField("az").f / -1, e.data.GetField("ay").f / -1);
            //add player to Players list
            players.Add(newPlayer);
            Debug.Log("Players count: " + players.Count);
            //make new UI indicator
            //thanks to user Dibbie at https://answers.unity.com/questions/1165465/how-to-instantiate-ui-elements-centered-around-an.html
            /*Vector3 canPos = can.transform.position; //middle pos
			float spot = players.Count;
			Vector3 pos = new Vector3(canPos.x * (spot*0.4f), canPos.y * 1.5f, canPos.z);
			GameObject newPhone = Instantiate(phone, pos, Quaternion.identity);
			newPhone.name = spot.ToString();
        	newPhone.GetComponent<Renderer>().material.color = new Color(e.data.GetField("r").f / 255f, e.data.GetField("b").f / 255f, e.data.GetField("g").f / 255f, 205f);
			newPhone.transform.SetParent(can.transform, false);
			RectTransform rt = newPhone.GetComponent<RectTransform>(); 
			rt.anchoredPosition.Set(pos.x, pos.y);
			*/
        }
        else //update that player's quaternion
        {
            //will this reference change the original? how do i learn more about references?
            check.thisQuat.eulerAngles = new Vector3(e.data.GetField("ax").f, e.data.GetField("az").f / -1, e.data.GetField("ay").f / -1);
        }

        //old stuff
        // Debug.Log(e.data);
        //rotate platform
        // platform.transform.eulerAngles = new Vector3(e.data.GetField("xAvg").f / 2f, e.data.GetField("zAvg").f / -2f, e.data.GetField("yAvg").f / -2f);
        //change platform color
        // platform.GetComponent<Renderer>().material.color = new Color(e.data.GetField("r").f / 255f, e.data.GetField("b").f / 255f, e.data.GetField("g").f / 255f, 255f);

        //now making quaternions to smooth
        // need to divide by 2f still?
        // z on phone is y on quat and vice versa
        // newQuat.rotation = SmoothQuaternions(e.data.GetField("ax").f, e.data.GetField("az").f, e.data.GetField("ay").f);
        // Debug.Log(newQuat.rotation);

        //need to do eulerangles instead of making new quaternion?
        //still need to flip y and z, and divide by a new variable
        //maybe need to divide by chillout after to fix jumpy?
        // newQuat.eulerAngles = new Vector3(e.data.GetField("ax").f / chillOut, e.data.GetField("az").f / -chillOut, e.data.GetField("ay").f / -chillOut);

        // newQuat.eulerAngles = new Vector3(e.data.GetField("ax").f, e.data.GetField("az").f / -1, e.data.GetField("ay").f / -1);
        // newQuat.eulerAngles /= chillOut;
    }

    private static Quaternion SmoothQuaternions(float x, float y, float z)
    {
        //do I need to keep an array? test first without and see
        float newX = x;
        float newY = y;
        float newZ = z;
        //need to set eulerangles, because quaternion.x is not the same thing
        return new Quaternion(newX, newY, newZ, 1);
    }

    private void DisconnectClient(SocketIOEvent e)
    {
        //check the players list for that player
        Player check = players.Find(x => x.id == e.data.GetField("id").str);
        if (check == null) //new player
        {
            Debug.Log("what happened");
        }
        else //remove the player from the list
        {
            Debug.Log("player left: " + check.id);
            //will this reference change the original? how do i learn more about references?
            players.Remove(check); //could it be that easy?...
        }
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
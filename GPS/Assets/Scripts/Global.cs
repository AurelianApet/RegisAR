using System.Collections.Generic;

public static class Global {
	public static bool bGPSStatus = true;
    public class GPS_pos
	{
        public int id;
        public string name;
        public string offerofday;
        public double latitude;
        public double longitude;
        public int category;
        public bool status;

		public GPS_pos(int bid, string bname, string bf, double lat, double lng, int bcat, 
            bool bstatus = true){
            id = bid;
            name = bname;
            offerofday = bf;
            longitude = lng;
			latitude = lat;
            category = bcat;
            status = bstatus;
		}
	}

    public static List<GPS_pos> markers = new List<GPS_pos>();
    public static List<bool> isArr = new List<bool>();
    public static List<string> info_string = new List<string>();
    public static GPS_pos defaultPos = new GPS_pos(1000, "Your Position", "Posicion actual", 0, 0, 0);
    public static GPS_pos curPos = defaultPos;
    public static string curInfo;
    public static bool delItem = false;
    /*
        "Posicion actual"
    public static GPS_pos pos4 = new GPS_pos(-99.1631254f, 19.4286648f, "Angel de la independencia");//Angel de la independencia 
    public static GPS_pos pos5 = new GPS_pos(-99.1627958f, 19.4286915f, "Reforma");//Reforma 222
    public static GPS_pos pos6 = new GPS_pos(-99.1723519f, 19.42482f, "Fuente Diana Cazadora");//Fuente Diana Cazadora
    public static GPS_pos defaultPos = new GPS_pos(-105.2f, 20.6f, "default position");
    public static GPS_pos pos1 = new GPS_pos(-105.2336377f, 20.6020451f, "PEMEX");//PEMEX 
    public static GPS_pos pos2 = new GPS_pos(-105.2340589f, 20.6043459f, "Pollo Feliz");//Pollo Feliz
    public static GPS_pos pos3 = new GPS_pos(-105.235831f, 20.6028163f, "Remax");//Remax
    */
    public static int zoom = 18;
    public static string push_string;
    public static string onesignal_appid = "888562c2-bec9-4b74-92fb-c813449cea53";
    public static string onesignal_userid = "8bfdf70f-373c-453b-81f3-6e6a6d7691c7";
    public static bool marker_set = false;
    public static bool markerSetted = false;

    public static bool map_set = false;
    public static bool gps_set = false;
    public static int dis = 75;
    public static bool isset_push = true;

    public static bool catclk = false;
    public static int selAddorEdit = 1;//1-add, 0-edit
    
    //current marker variables
    public static string curMarkerName;
    public static string curMarkerOfferofday;
    public static int curMarkerCategory = -1;
    public static bool curMarkerStatus = true;
    public static double curMarkerLat;
    public static double curMarkerLng;
    public static int curMarkerId = -1;

    //api information
    public static string api_domain = "https://regis.fenixsoft.com/";
    public static string getMarkersApi = "getBusApi.php";
    public static string addMarkerApi = "addBusApi.php";
    public static string editMarkerApi = "editBusApi.php";
    public static string delMarkerApi = "delBusApi.php";
    public static string getCategoryApi = "getCategoryApi.php";
    public static string setCategoryApi = "setCategoryApi.php?list=";
}

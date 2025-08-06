using UnityEngine;

public class DeserializeTest : MonoBehaviour
{
    // so this creates a class that match the structure off my test json. 
    [System.Serializable] 
    public class Person
    {
        public string name; // the json starts with a name
        public int age; // the json ends with a age
    }
    [System.Serializable]
    public class PersonOnlyAge
    {
        public int age;
    }        

    void Start()
    {
        string json = "{\"name\": \"Totte\", \"age\": 31}"; // this simulate a json file. it is a string but follows a data structure format. 
        //json data structure = { "key": value }  , used as divider for mutliple keys and values pairs.  \ in above is used to be able to write ".

        PersonOnlyAge personOnlyAge = JsonUtility.FromJson<PersonOnlyAge>(json); //Deserialize the Json string into a object of type Person

        //Debug.Log("Name: "+ person.name);
        Debug.Log("Age: "+ personOnlyAge.age);

    }
}

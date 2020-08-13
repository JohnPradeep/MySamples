// rapidjson/example/simpledom/simpledom.cpp`
#include "include/rapidjson/document.h"
#include "include/rapidjson/writer.h"
#include "include/rapidjson/stringbuffer.h"
#include <iostream>

using namespace rapidjson;

struct userRecord {
    char              name[128];
};
int process(Document& Value_, char* destination);

int main() {
    // 1. Parse a JSON string into DOM.
    const char* json = "{ \"Transaction\":\"AddNewUser\",\"IP\" : \"192.168.0.102\",\"User\" : {\"Group\":\"demoforex\",\"Name\" : \"pradeep\",\"Password\" : \"test@1234\",\"Investor\" : null,\"Email\" : \"test@gmail.com\",\"Country\" : null,\"State\" : null,\"City\" : null,\"Address\" : null,\"Commen\" : null,\"Phone\" : null,\"PhonePassword\" : null,\"Status\" : null,\"ZipCode\" : null,\"Id\" : null,\"Leverage\" : 1,\"Agent\" : 0,\"SendReports\" : false,\"Deposit\" : 0.0} }";
    const int size = strlen(json);
    char buffernew[1000];
    //char *buffer = (char*)malloc(size);
    _snprintf(buffernew, size,"%s",json);
   // const char* newValue = buffer;
    Document d;
    d.Parse(json);
    //if (d.IsObject())
    //{
    //    const char* result = d["Transaction"].GetString();


    //    if (strcmp(d["Transaction"].GetString(), "AddNewUser") == 0) {
    //        // 2. Modify it by DOM.
    //        Value& s = d["stars"];
    //        s.SetInt(s.GetInt() + 1);

    //        // 3. Stringify the DOM
    //        StringBuffer buffer;
    //        Writer<StringBuffer> writer(buffer);
    //        d.Accept(writer);

    //        // Output {"project":"rapidjson","stars":11}
    //        std::cout << buffer.GetString() << std::endl;
    //    }
    //}
   
    rapidjson::Document document;

    // define the document as an object rather than an array
    document.SetObject();

    // create a rapidjson array type with similar syntax to std::vector
    rapidjson::Value array(rapidjson::kArrayType);

    // must pass an allocator when the object may need to allocate memory
    rapidjson::Document::AllocatorType& allocator = document.GetAllocator();
    char nameObject[16] = "pradeep";
    userRecord record = { 0 };
    //strcpy(record.name, "Test");
    //char result12[128];
    //strcpy(result12, record.name);
    std::string groupName = std::string(record.name);
    strcpy(record.name, "UpdatedValue");
    for (int i = 0; i < 3; i++) {
        char result12[128];
        record = { 0 };
        if (i == 0)
            strcpy(record.name, "UpdatedValue");
        if (i == 1)
            strcpy(record.name, "First");
        else if (i == 2)
            strcpy(record.name, "Second");
        std::string s(strcpy(result12, record.name));
        rapidjson::Value strVal;
        strVal.SetString(s.c_str(), s.length(), allocator);
        array.PushBack(strVal, allocator);
        //result12[0] = 0;
    }
    // chain methods as rapidjson provides a fluent interface when modifying its objects
    array.PushBack("hello", allocator).PushBack("world", allocator);//"array":["hello","world"]
    array.PushBack("Third", allocator);
    document.AddMember("Name", "123548", allocator);
    if (document["Name"].GetString() == "123548") 
    {
        userRecord user = { 0 };
        Value& s = document["Name"];
    }
    userRecord user = { 0 };
    Value& s = document["Name"];
    if (!s.IsNull())
    {
        document.AddMember("Rollnumer", 2, allocator);
    }
    if (document["Name"].IsNull() == true)
    {
        document.AddMember("Rollnumer", 2, allocator);
    }
    if(document.HasMember("Name") && !document["Name"].IsNull()) {
       /* Value& s = document["NameTest"];
        if (!s.IsNull())*/
            strcpy(user.name, document["Name"].GetString());
    }
    document.AddMember("Rollnumer", 2, allocator);
    document.AddMember("array", array, allocator);
    document.AddMember("Test", StringRef(buffernew), allocator);
    // create a rapidjson object type
    rapidjson::Value object(rapidjson::kObjectType);
    object.AddMember("Math", "50", allocator);
    object.AddMember("Science", "70", allocator);
    object.AddMember("English", "50", allocator);
    object.AddMember("Social Science", "70", allocator);
    document.AddMember("Marks", object, allocator);
     char* mark;
    ////	fromScratch["object"]["hello"] = "Yourname";
    //if (document["Marks"].IsObject())
    //{
    //    Value& v = document["Marks"];
    //     mark = v["Math"].GetString();

    //}
    process(document, user.name);
    StringBuffer strbuf;
    Writer<StringBuffer> writer(strbuf);
    document.Accept(writer);
    const char* result = strbuf.GetString();
    strbuf.Flush();
    writer.Flush();
    allocator.Clear();
    std::cout << result << std::endl;

    return 0;
}

int process(Document& document, char *destination)
{
    const char* mark;
    //	fromScratch["object"]["hello"] = "Yourname";
    if (document["Marks"].IsObject())
    {
        Value& v = document["Marks"];
        mark = v["Math"].GetString();
        strcpy_s(destination, sizeof(mark), mark);

    }
    return 0;
}
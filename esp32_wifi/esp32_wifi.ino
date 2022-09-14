#include <WiFi.h>

const char ssid[] = "ESP32AP-WiFi";
const char pass[] = "esp32apwifi";
const IPAddress ip(192,168,30,3);
const IPAddress subnet(255,255,255,0);

const char html[] =
"<!DOCTYPE html><html lang='ja'><head><meta charset='UTF-8'>\
<style>input {margin:8px;width:80px;}\
div {font-size:16pt;color:red;text-align:center;width:400px;border:groove 40px orange;}</style>\
<title>WiFi_Car Controller</title></head>\
<body><div><p>Tank Controller</p>\
<form method='get'>\
<input type='submit' name='le' value='左' />\
<input type='submit' name='fo' value='前' />\
<input type='submit' name='ri' value='右' /><br>\
<input type='submit' name='st' value='停止' /><br>\
<input type='submit' name='bl' value='後左' />\
<input type='submit' name='ba' value='後ろ' />\
<input type='submit' name='br' value='後右' /><br><br>\
</form></div></body></html>";

WiFiServer server(80);

void setup()
{
    Serial.begin(115200);

    WiFi.softAP(ssid, pass);
    delay(100);
    WiFi.softAPConfig(ip, ip, subnet);

    IPAddress myIP = WiFi.softAPIP();

    server.begin();

    Serial.print("SSID: ");
    Serial.println(ssid);
    Serial.print("AP IP address: ");
    Serial.println(myIP);
    Serial.println("Server start!");
}

void loop(){
    WiFiClient client = server.available();

    if (client) {
        String currentLine = "";
        Serial.println("New Client.");

        while (client.connected()) {
            if (client.available()) {
                char c = client.read();
                Serial.write(c);
                if (c == '\n') {
                    if (currentLine.length() == 0) {
                        client.println("HTTP/1.1 200 OK");
                        client.println("Content-type:text/html");
                        client.println();

                        client.print(html);
                        client.println();
                        break;
                    } else {
                        currentLine = "";
                    }
                } else if (c != '\r') {
                    currentLine += c;
                }
                
                if (currentLine.endsWith("GET /?help")) {
                    Serial.println("\nhelp");
                }

//                Serial.println(currentLine);
//                delay(10);
            }
        }
        client.stop();
        Serial.println("Client Disconnected.");
    }
}

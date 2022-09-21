#include <WiFi.h>

const char ssid[] = "ESP32AP-WiFi";
const char pass[] = "esp32apwifi";
const IPAddress ip(192, 168, 30, 3);
const IPAddress subnet(255, 255, 255, 0);

const char html[] =
  "<!DOCTYPE html><html lang='ja'><head><meta charset='utf-8'></head><body>\
  <a href='/?true'>正転</a><br /><a href='/?false'>逆転</a><br /><a href='/?zero'>停止</a></body></html>";

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


  pinMode(2, OUTPUT);
  pinMode(0, OUTPUT);
  pinMode(12, OUTPUT);
  pinMode(14, OUTPUT);

}

void loop() {
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

        if (currentLine.endsWith("GET /?true")) {
          digitalWrite(2, LOW);
          digitalWrite(0, HIGH);
          digitalWrite(12, LOW);
          digitalWrite(14, HIGH);
        }
        if (currentLine.endsWith("GET /?false")) {
          digitalWrite(0, LOW);
          digitalWrite(2, HIGH);
          digitalWrite(12, HIGH);
          digitalWrite(14, LOW);
        }
        if (currentLine.endsWith("GET /?zero")) {
          digitalWrite(0, LOW);
          digitalWrite(2, LOW);
          digitalWrite(12, LOW);
          digitalWrite(14, LOW);
        }

        //                Serial.println(currentLine);
        //                delay(10);
      }
    }
    client.stop();
    Serial.println("Client Disconnected.");
  }
}

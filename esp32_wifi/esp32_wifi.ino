#include <WiFi.h>

const char ssid[] = "ESP32-VR-01";
const char pass[] = "hogehoge";
const IPAddress ip(192, 168, 30, 3);
const IPAddress subnet(255, 255, 255, 0);

// 現在の状態を保存する?
const int motor_pin[2][2] = {{2, 4}, {16, 17}};
int motor_endtime[2][2] = {{0, 0}, {0, 0}};

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

  for (int id = 0; id < 2; ++id) {
    for (int pin = 0; pin < 2; ++pin) {
      pinMode(motor_pin[id][pin], OUTPUT);
      digitalWrite(motor_pin[id][pin], LOW);
    }
  }

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

void loop() {
  int t_ms = millis();
  if (t_ms % 1000 <= 10) Serial.println(t_ms);
  for (int id = 0; id < 2; ++id) {
    for (int pin = 0; pin < 2; ++pin) {
      if (motor_endtime[id][pin] <= t_ms) {
        Serial.println(motor_endtime[id][pin] + "<=" + t_ms);
        digitalWrite(motor_pin[id][pin], LOW);
      }
    }
  }
  WiFiClient client = server.available();

  if (client) {
    String currentLine = "";
    Serial.println("New Client.");

    while (client.connected()) {
      if (client.available()) {
        char c = client.read();
        //        Serial.write(c);
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

        if (currentLine.endsWith("GET /t")) {
          digitalWrite(motor_pin[0][0], HIGH);
          digitalWrite(motor_pin[0][1], LOW);
          Serial.println("\n\nt\n\n");
          motor_endtime[0][0] = t_ms + 10000;
        }
        if (currentLine.endsWith("GET /f")) {
          digitalWrite(motor_pin[0][0], LOW);
          digitalWrite(motor_pin[0][1], HIGH);
          Serial.println("\n\nf\n\n");
          motor_endtime[0][1] = t_ms + 10000;
        }

        //                Serial.println(currentLine);
        //                delay(10);
      }
    }
    client.stop();
    Serial.println("Client Disconnected.");
  }
}

#include <Wire.h>
#include <AHTxx.h>
#if defined(ESP8266)
#include <ESP8266WiFi.h>
#endif

float ahtTemp;
float ahtHum;    
char buff[50];

AHTxx aht20(AHTXX_ADDRESS_X38, AHT2x_SENSOR); //sensor address, sensor type



/**************************************************************************/
/*
    setup()

    Main setup
*/
/**************************************************************************/
void setup()
{
  #if defined(ESP8266)
  WiFi.persistent(false);  //disable saving wifi config into SDK flash area
  WiFi.forceSleepBegin();  //disable AP & station by calling "WiFi.mode(WIFI_OFF)" & put modem to sleep
  #endif

  Serial.begin(115200);
  
  while (aht20.begin() != true)
  {
    Serial.println(F("AHT2x not connected or fail to load calibration coefficient")); //(F()) save string to flash & keeps dynamic memory free

    delay(5000);
  }

  //Serial.println(F("AHT20 OK"));

  //Wire.setClock(400000); //experimental I2C speed! 400KHz, default 100KHz
}


/**************************************************************************/
/*
    loop()

     Main loop
*/
/**************************************************************************/
void loop()
{
  ahtTemp = aht20.readTemperature(); //read 6-bytes via I2C, takes 80 milliseconds
  ahtHum = aht20.readHumidity(); //read another 6-bytes via I2C, takes 80 milliseconds

  sprintf(buff, "<<%f||||%f>>", ahtTemp, ahtHum);
  Serial.println(buff);

  delay(2000);
  

}
/**************************************************************************/
/*
    printStatus()

    Print last command status
*/
/**************************************************************************/
void printStatus()
{
  switch (aht20.getStatus())
  {
    case AHTXX_NO_ERROR:
      Serial.println(F("no error"));
      break;

    case AHTXX_BUSY_ERROR:
      Serial.println(F("sensor busy, increase polling time"));
      break;

    case AHTXX_ACK_ERROR:
      Serial.println(F("sensor didn't return ACK, not connected, broken, long wires (reduce speed), bus locked by slave (increase stretch limit)"));
      break;

    case AHTXX_DATA_ERROR:
      Serial.println(F("received data smaller than expected, not connected, broken, long wires (reduce speed), bus locked by slave (increase stretch limit)"));
      break;

    case AHTXX_CRC8_ERROR:
      Serial.println(F("computed CRC8 not match received CRC8, this feature supported only by AHT2x sensors"));
      break;

    default:
      Serial.println(F("unknown status"));    
      break;
  }
}
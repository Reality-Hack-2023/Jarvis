#include <BTAddress.h>
#include <BTAdvertisedDevice.h>
#include <BTScan.h>
#include <BluetoothSerial.h>

/*
  ATTRIBUTION:
    - example code from seeed studio: https://wiki.seeedstudio.com/Grove-Ear-clip_Heart_Rate_Sensor/#specifications
    - ESP32 BluetoothSerial tutorial: https://vhb2022.notion.site/How-to-make-ESP32-talk-to-a-computer-over-Bluetooth-6f5b4c1a84c24f90b0318011ec527239
*/

#define INTR_BUFFER_SIZE 5

BluetoothSerial BTSerial;

#define USE_BT 1

#if USE_BT
  #define MySerial BTSerial
#else
  #define MySerial Serial
#endif

unsigned char counter;
unsigned delta_since_last_intr;

unsigned long intr_millis[INTR_BUFFER_SIZE + 1];
unsigned long intr_millis_delta[INTR_BUFFER_SIZE];
bool intr_millis_valid = true;

unsigned int heart_rate;
bool heart_rate_updated = false;

const int max_heartpulse_duty = 2000;

void update_delta_from_running_sum() {
  for (int i = 1; i < INTR_BUFFER_SIZE + 1; i++) {
    intr_millis_delta[i-1] = intr_millis[i] - intr_millis[i-1];
  }
}

double rmssd() {
  double rmssd = 0;

  update_delta_from_running_sum();

  for (int i = 1; i < INTR_BUFFER_SIZE - 1; i++) {
    rmssd += pow(abs((long)(intr_millis_delta[i] - intr_millis_delta[i-1])), 2);
  }

  rmssd = sqrt(rmssd / (INTR_BUFFER_SIZE - 1));
  return rmssd;
}


void setup()
{
#if USE_BT
  BTSerial.begin("jARvis");
#else
  Serial.begin(9600);
#endif

  MySerial.println("Pausing for 5 seconds");
  delay(5000);
  arrayInit();
  MySerial.println("Start monitor");
  attachInterrupt(digitalPinToInterrupt(2), interrupt, RISING);
}
void loop()
{
  if (!heart_rate_updated) return;

  double hrv = rmssd();
  MySerial.print("HRV:\t");
  MySerial.println(hrv);
  MySerial.print("HR:\t");
  MySerial.println(heart_rate);
  heart_rate_updated = false;                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                    
}

void update_heart_rate() {
  if (intr_millis_valid) {
    heart_rate = 240000/(intr_millis[INTR_BUFFER_SIZE]-intr_millis[0]); //INTR_BUFFER_SIZE * 60 * 1000 / total_time
    heart_rate_updated = true;
  } else {
    // run through another iteration, assume valid unless delta since last exceeds 2s
    //intr_millis_valid = true;
  }
}

void interrupt()
{
  intr_millis[counter] = millis();
    
  switch(counter) {
  case 0:
    delta_since_last_intr = intr_millis[counter]-intr_millis[INTR_BUFFER_SIZE];
    break;
  default:
    delta_since_last_intr = intr_millis[counter]-intr_millis[counter-1];
    break;
  }

  if (delta_since_last_intr > max_heartpulse_duty) {
    intr_millis_valid = false;
    counter = 0;
    MySerial.println("Resetting dataset");
    arrayInit();
  }
  
  if (counter == INTR_BUFFER_SIZE && intr_millis_valid) {
    counter = 0;
    update_heart_rate();
  } else if (counter != INTR_BUFFER_SIZE && intr_millis_valid) {
    counter++;
  } else {
    counter = 0;
    intr_millis_valid = 1;
  }
}

void arrayInit()
{
  for(unsigned char i=0; i < INTR_BUFFER_SIZE;i ++) {
    intr_millis[i]=0;
  }
  intr_millis[INTR_BUFFER_SIZE]=millis();
}

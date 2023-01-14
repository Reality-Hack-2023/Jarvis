#include <BTAddress.h>
#include <BTAdvertisedDevice.h>
#include <BTScan.h>
#include <BluetoothSerial.h>

#include "soc/rtc_wdt.h"

/*
  ATTRIBUTION:
    - example code from seeed studio: https://wiki.seeedstudio.com/Grove-Ear-clip_Heart_Rate_Sensor/#specifications
    - ESP32 BluetoothSerial tutorial: https://vhb2022.notion.site/How-to-make-ESP32-talk-to-a-computer-over-Bluetooth-6f5b4c1a84c24f90b0318011ec527239
*/

#define INTR_BUFFER_SIZE 5

BluetoothSerial BTSerial;

#define USE_BT 0

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

const int min_heartpulse_duty = 250;
const int max_heartpulse_duty = 2000;

void update_delta_from_running_sum() {
  for (int i = 1; i < INTR_BUFFER_SIZE + 1; i++) {
    intr_millis_delta[i-1] = intr_millis[i] - intr_millis[i-1];
  }
}

#define STRESS_LEVEL_CLASS_ARR_LEN 3
int stress_level_class_arr[] = {25, 50, 100};

int get_stress_level_from_hrv(double hrv) {
  /*
    0: <25
    1: 25<x<=50
    2: 50<x<=100
    3: >100
  */

  if (hrv < stress_level_class_arr[0]) {
    return 0;
  } else if (hrv > stress_level_class_arr[STRESS_LEVEL_CLASS_ARR_LEN - 1]) {
    return STRESS_LEVEL_CLASS_ARR_LEN;
  } else {
    for (int i = 0; i < STRESS_LEVEL_CLASS_ARR_LEN; i++) {
      // out of bounds for last elem but will never reach this case 
      if (hrv > stress_level_class_arr[i] && hrv <= stress_level_class_arr[i+1]) {
        return i + 1;
      }
    }
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
  int stress_level = get_stress_level_from_hrv(hrv);
  MySerial.print("HRV:\t");
  MySerial.println(hrv);
  MySerial.print("HR:\t");
  MySerial.println(heart_rate);
  MySerial.print("SL:\t");
  MySerial.println(stress_level);
  heart_rate_updated = false;                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                    
}

void update_heart_rate() {
  heart_rate = 240000/(intr_millis[INTR_BUFFER_SIZE]-intr_millis[0]); //INTR_BUFFER_SIZE * 60 * 1000 / total_time
  heart_rate_updated = true;
}

void interrupt()
{
  unsigned long curr_millis = millis();

  switch(counter) {
  case 0:
    delta_since_last_intr = curr_millis-intr_millis[INTR_BUFFER_SIZE];
    break;
  default:
    delta_since_last_intr = curr_millis-intr_millis[counter-1];
    break;
  }

  if (delta_since_last_intr > max_heartpulse_duty) {
    // 2 seconds since last heart beat - throw away current measurements
    intr_millis_valid = false;
    counter = 0;
    arrayInit();
  } else if (delta_since_last_intr < min_heartpulse_duty) {
    // discard this interrupt as it occurred <250ms since the last one
    return;
  }

  intr_millis[counter] = curr_millis;
  
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
  for (unsigned char i=0; i < INTR_BUFFER_SIZE; i++) {
    intr_millis[i] = 0;
  }
  intr_millis[INTR_BUFFER_SIZE] = millis();
}

//Board = Arduino Uno
#define ARDUINO 103
#define __AVR_ATmega328P__
#define F_CPU 16000000L
#define __AVR__
#define __cplusplus
#define __attribute__(x)
#define __inline__
#define __asm__(x)
#define __extension__
#define __ATTR_PURE__
#define __ATTR_CONST__
#define __inline__
#define __asm__ 
#define __volatile__
#define __builtin_va_list
#define __builtin_va_start
#define __builtin_va_end
#define __DOXYGEN__
#define prog_void
#define PGM_VOID_P int
#define NOINLINE __attribute__((noinline))

typedef unsigned char byte;
extern "C" void __cxa_pure_virtual() {}

void readAndReportData(byte address, int theRegister, byte numBytes);
void outputPort(byte portNumber, byte portValue, byte forceSend);
void checkDigitalInputs(void);
void setPinModeCallback(byte pin, int mode);
void analogWriteCallback(byte pin, int value);
void digitalWriteCallback(byte port, int value);
void reportAnalogCallback(byte analogPin, int value);
void reportDigitalCallback(byte port, int value);
void sysexCallback(byte command, byte argc, byte *argv);
void enableI2CPins();
void disableI2CPins();
void systemResetCallback();
//already defined in arduno.h
//already defined in arduno.h

#include "D:\NB12012\Downloads\Instalacoes\Arduino\arduino-1.0.3\hardware\arduino\variants\standard\pins_arduino.h" 
#include "D:\NB12012\Downloads\Instalacoes\Arduino\arduino-1.0.3\hardware\arduino\cores\arduino\arduino.h"
#include "D:\Projectos\Robotica\Suricata\Suricata\SonarExFirmata\SonarExFirmata.ino"

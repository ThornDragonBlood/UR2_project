const int ledPin = 13; // the pin that the LED is attached to
const byte buffSize = 40;
unsigned int inputBuffer[buffSize];
const char startMarker = '<';
const char endMarker = '>';
byte bytesRecvd = 0;
boolean readInProgress = false;
boolean newDataFromPC = false;
byte coordinates[3];
double temp[3];
double location[3];
const double circumference = 32;
const int IN12 = 5;
const int IN22 = 4;
const int IN32 = 7;
const int IN42 = 6;
const int IN11 = 13;
const int IN21 = 12;
const int IN31 = 10;
const int IN41 = 9;
const int INput1 = 2;
const int INput2 = 3;
int disstance = 0;
volatile byte state = LOW;
int incomingbite = 0;
int count = 0;
int countm = 0;
double tem1 = 0;
double tem2 = 0;

void setup() 
{
  // put your setup code here, to run once:
  Serial.begin(115200);
  pinMode(IN12, OUTPUT);
  pinMode(IN22, OUTPUT);
  pinMode(IN32, OUTPUT);
  pinMode(IN42, OUTPUT);
  pinMode(IN11, OUTPUT);
  pinMode(IN21, OUTPUT);
  pinMode(IN31, OUTPUT);
  pinMode(IN41, OUTPUT);
  pinMode(INput1, INPUT_PULLUP);
  location[0]=0;
  location[1]=0;
  location[2]=0;
}

// The primary loop of the program
void loop() 
{
  // put your main code here, to run repeatedly:
  getDataFromPC();
  if(newDataFromPC)
  {
    sendSuspendCmd();
    prossesbyte();
    rotate();
    moveboom();
    lowerende();
    delay(200);
    digitalWrite(IN12, LOW);
    digitalWrite(IN22, HIGH);
    delay(200);
    raseende();
    reset();
    prossesbyte();
    moveboom();
    rotate();
    stack();
    prossesbyte();
    rotate();
    moveboom();
    lowerende();
    magnicoff();
    raseende();
    prossesbyte();
    moveboom();
    rotate();
    reset();
    prossesbyte();
    moveboom();
    rotate();
    
    
    sendEnableCmd();
    sendCoordinatesToPC();
    newDataFromPC = false;
  }
}

// Takss the input bytes and converts them to a form the movement code can understand
void prossesbyte()
{
  temp[0] = coordinates[0] * 350.0;
  temp[0] = temp[0] / circumference;
  temp[1] = coordinates[1] * 840.0;
  temp[1] = temp[1] / 360.0;
  temp[2] = coordinates[2];
  
}

// turns off the magnet and detaches the metal part from the magnet
void magnicoff()
{
  digitalWrite(IN12, LOW);
  digitalWrite(IN22, LOW);
  digitalWrite(IN32, LOW);
  digitalWrite(IN42, HIGH);
  delay(20);
  digitalWrite(IN32, HIGH);
  digitalWrite(IN42, LOW);
  delay(20);
  digitalWrite(IN32, LOW);
  digitalWrite(IN42, LOW);
}

// lowers the endeffecter
void lowerende()
{
  int tranfer = 0;
  while(tranfer <= 14)
  {
    digitalWrite(IN32, HIGH);
    digitalWrite(IN42, LOW);
    delay(50);
    digitalWrite(IN32, LOW);
    digitalWrite(IN42, LOW);
    delay(50);
    tranfer++;
  }
}

// raises the endeffecter
void raseende()
{
  int tranfer = 0;
  while(tranfer <= 16)
  {
    digitalWrite(IN32, LOW);
    digitalWrite(IN42, HIGH);
    delay(50);
    digitalWrite(IN32, LOW);
    digitalWrite(IN42, LOW);
    delay(50);
    tranfer++;
  }
}

// input the to the code the home position
void reset()
{
  coordinates[1] = 0;
  coordinates[0] = 0;
}

//set position to the drop points of the parts
void stack()
{
  if(temp[2] == 1)
  {
    coordinates[1] = 20;
    coordinates[0] = 20;
  }else
  {
    coordinates[1] = 40;
    coordinates[0] = 50;
    temp[2] = 1;
  }
}

// mover the boom arm of the robot
void moveboom()
{ 
  tem2 = 0;
  if(location[0] > temp[0])
  {
    tem2 = location[0]-temp[0];
    while(tem2 > 0)
    {
      //sendCoordinatesToPC1();
      if(tem2 > 20)
      {
        digitalWrite(IN11, HIGH);
        digitalWrite(IN21, LOW);
        delay(24);
        digitalWrite(IN11, LOW);
        digitalWrite(IN21, LOW);
        delay(100);
        tem2 = tem2-20;      
      }else
      {
        digitalWrite(IN11, LOW);
        digitalWrite(IN21, HIGH);
        delay(tem2);
        digitalWrite(IN11, LOW);
        digitalWrite(IN21, LOW);
        delay(100);
        tem2 = 0;
      }
    }
  } else 
  if(location[0] < temp[0])
  {
    tem2 = temp[0]-location[0];
    //sendCoordinatesToPC1();
    while(tem2 > 0)
    {
      //sendCoordinatesToPC1();
      if(tem2 > 20)
      {
        //sendCoordinatesToPC1();
        digitalWrite(IN11, LOW);
        digitalWrite(IN21, HIGH);
        delay(20);
        digitalWrite(IN11, LOW);
        digitalWrite(IN21, LOW);
        delay(100);
        tem2 = tem2-20;      
      }else
      {
        digitalWrite(IN11, LOW);
        digitalWrite(IN21, HIGH);
        delay(tem2);
        digitalWrite(IN11, LOW);
        digitalWrite(IN21, LOW);
        delay(100);
        tem2 = 0;
      }
    }
  }
  location[0] = temp[0];
}

// rotates the turret of the robot
void rotate()
{ 
  tem1 = 0;
  if(location[1] > temp[1])
  {
    tem1 = location[1]-temp[1];
    //sendCoordinatesToPC1();
    while(tem1 > 0)
    {
      if(tem1 > 11)
      {
        digitalWrite(IN31, LOW);
        digitalWrite(IN41, HIGH);
        delay(13);
        digitalWrite(IN31, LOW);
        digitalWrite(IN41, LOW);
        delay(200);
        tem1 = tem1-11;      
      }else
      {
        digitalWrite(IN31, LOW);
        digitalWrite(IN41, HIGH);
        delay(tem1);
        digitalWrite(IN31, LOW);
        digitalWrite(IN41, LOW);
        delay(200);
        tem1 = 0;
      }
    }
  } else 
  if(location[1] < temp[1])
  {
    tem1 = temp[1]-location[1];
    //sendCoordinatesToPC1();
    while(tem1 > 0)
    {
      if(tem1 > 11)
      {
        digitalWrite(IN31, HIGH);
        digitalWrite(IN41, LOW);
        delay(11);
        digitalWrite(IN31, LOW);
        digitalWrite(IN41, LOW);
        delay(200);
        tem1 = tem1-11;      
      }else
      {
        digitalWrite(IN31, HIGH);
        digitalWrite(IN41, LOW);
        delay(tem1);
        digitalWrite(IN31, LOW);
        digitalWrite(IN41, LOW);
        delay(200);
        tem1 = 0;
      }
    }
  }
  location[1] = temp[1];
}

// turn off the abilte for the code to input from the computer
void sendSuspendCmd()
{
  // send the suspend-true command
  Serial.println("<S1>");
}

// turn on the abilte for the code to input from the computer
void sendEnableCmd()
{
  // send the suspend-false command
  Serial.println("<S0>");
}

//sends back the input code back to the computer
void sendCoordinatesToPC()
{
  // send the point data to the PC
  Serial.print("<P");
  Serial.print(coordinates[0]);
  Serial.print(",");
  Serial.print(coordinates[1]);
  Serial.print(",");
  Serial.print(coordinates[2]);
  Serial.println(">");
}

//debug code to determan what the code is doing
void sendCoordinatesToPC1()
{
  // send the point data to the PC
  Serial.print("<t");
  Serial.print(coordinates[1]);
  Serial.print(",");
  Serial.print(String(temp[0]));
  Serial.print(",");
  Serial.print(tem2);
  Serial.println(">");
}

// alternative to the readBytes function:
// Get input from the computer
void getDataFromPC() 
{
  // receive data from PC and save it into inputBuffer
  if(Serial.available() > 0) 
  {
    char x = Serial.read();
    // the order of these IF clauses is significant
    if (x == endMarker) 
    {
      readInProgress = false;
      newDataFromPC = true;
      inputBuffer[bytesRecvd] = 0;
      coordinates[0] = inputBuffer[0];
      coordinates[1] = inputBuffer[1];
      coordinates[2] = inputBuffer[2];
    }
    if(readInProgress) 
    {
      inputBuffer[bytesRecvd] = x;
      bytesRecvd ++;
      if (bytesRecvd == buffSize) 
      {
        bytesRecvd = buffSize - 1;
      }
    }
    if (x == startMarker) 
    {
      bytesRecvd = 0;
      readInProgress = true;
    }
  }
}

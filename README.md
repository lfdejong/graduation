# Demo game met kussen controller

 ![image](https://github.com/user-attachments/assets/cc6d563e-ca28-4416-bdd9-ff6a5c512522)

Dit project maakt deel uit van mijn afstudeer project wat als doel heeft om de speler in beweging te zetten door actief te zitten tijdens een game.

## Installeren

Vereisten zijn de Unity Editor en  Arduino IDE. Voor het spelen van de game is een externe controller nodig gemaakt van een Arduino en vier druksensoren.
Download het project via Github en open het project binnen Unity Versie 2022.3.8f1. of hoger.

Om gebruik te maken van de Arduino is het belangrijk om binnen het project de Api Compatibility Level op .NET Framework te zetten.
Deze is te vinden op:
Edit -> Project Settings -> Player -> Configuration - Api Compatability Level
![image](https://github.com/user-attachments/assets/6de686c9-e2c9-4d12-b366-72587db0fe48)


## Packages
Binnen het project zijn meerdere packages gebruikt en deze staan in het project folder, voor het geval deze niet zijn toegevoegd binnen het project staan ze hieronder.
* Cinemachine
* Input system
* Probuilder
* TextMeshPro

### Asset packages
* [Forever - Endless Runner Engine](https://assetstore.unity.com/packages/tools/game-toolkits/forever-endless-runner-engine-140926#publisher)
* [Skybox Series Free](https://assetstore.unity.com/packages/2d/textures-materials/sky/skybox-series-free-103633?srsltid=AfmBOoqbS4NV_woiS6JdoLAZTvnhMog7TXWq76hqZjCNY4kjgy6rwj4U)
* [Kenney Nature Kit](https://kenney.nl/assets/nature-kit)
* [Kenney Pattern Pack](https://kenney.nl/assets/pattern-pack)
* [MiniGame Music Pack – JDSherbert](https://itch.io/queue/c/2618614/music-bgm-packs?game_id=882410)

## Arduino en sensoren

Binnen het project is gebruik gemaakt van Arduino Uno en vier vierkante druksensoren. De druksensoren zijn op een harde ondergrond geplaatst. Neem rekening met de volgorde, sensor 1 leidt naar pin 0 - 2 naar pin 1 - 3 naar pin 2 en 4 naar pin 3.

![image](https://github.com/user-attachments/assets/b08030d7-b9de-4fb3-92de-f27fb42535bf)

Hieronder is een diagramen die de bedrading van de Arduino vertoont. 

![image](https://github.com/user-attachments/assets/73397db9-96c8-4800-bc26-35663116db65)


## Arduino Code 

Binnen Arduino IDE check onder tools of de juiste Arduino en poort zijn aangevinkt. Zo niet verander deze naar Arduino UNO en COM3 
![image](https://github.com/user-attachments/assets/fba6f49d-ceb5-4659-bb00-5310a4432388)

De onderstaande code zorgt ervoor dat de data van de sensoren wordt geprint door de Arduino. Let erop dat de serial code 9600 is. Deze code en de serial poort naam COM3 moeten overeen komen in zowel Arduino IDE als in de Unity Code.

```
// Define the analog pins for the force sensors
const int forceSensor1Pin = A0;
const int forceSensor2Pin = A1;
const int forceSensor3Pin = A2;
const int forceSensor4Pin = A3;

void setup() {
  // Initialize the serial communication at 9600 baud rate

  Serial.begin(9600);
  Serial.println(millis()); // Records time based on when the arduino began running
}

void loop() {
  

  // Read the analog values from each force sensor
  int forceSensor1Value = analogRead(forceSensor1Pin);
  int forceSensor2Value = analogRead(forceSensor2Pin);
  int forceSensor3Value = analogRead(forceSensor3Pin);
  int forceSensor4Value = analogRead(forceSensor4Pin);

  // Send the values over serial in CSV format
  Serial.print(forceSensor1Value);
  Serial.print(",");
  Serial.print(forceSensor2Value);
  Serial.print(",");
  Serial.print(forceSensor3Value);
  Serial.print(",");
  Serial.print(forceSensor4Value);
  Serial.println(); // End the line for the next set of values

  // Add a small delay for stability (optional)
  delay(10);  
}
```
Upload deze code naar de Arduino.
![image](https://github.com/user-attachments/assets/7de26ab2-4840-4dca-9cc5-fbb9fa032ceb)

## Unity Script

Binnen Unity staat de script Arduino Input binnen de folder Assets/scripts/Movement. Let bij dit script op dat System.IO.Ports wordt gebruikt binnen de code. 

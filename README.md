# Zamirathe_Feast
Feast Mod for Unturned requiring Rocket

Here is my take on the old Feast that was available for 2.2.5.  My version, you have a little bit more configuration options. I have included a folder with a default configuration file.  Please use it and modify it.  An example I used during testing is provided below as well.

```xml
<?xml version="1.0" encoding="utf-8"?>
<FeastConfiguration xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
  <Enabled>true</Enabled>
  <minDropTime>600</minDropTime>
  <maxDropTime>900</maxDropTime>
  <dropRadius>20</dropRadius>
  <minItemsDrop>10</minItemsDrop>
  <maxItemsDrop>25</maxItemsDrop>
  <skyDrop>false</skyDrop>
  <skyRadius>10</skyRadius>
  <Items>
    <FeastItem>
      <Id>66</Id>
      <Name>Cloth</Name>
      <Chance>10</Chance>
      <Locations>
        <string>all</string>
      </Locations>
    </FeastItem>
    <FeastItem>
      <Id>43</Id>
      <Name>Military Ammunition Box</Name>
      <Chance>10</Chance>
      <Locations>
        <string>Ontario</string>
        <string>New Brunswick</string>
      </Locations>
    </FeastItem>
    <FeastItem>
      <Id>44</Id>
      <Name>Civilian Ammunition Box</Name>
      <Chance>10</Chance>
      <Locations>
        <string>Ontario</string>
        <string>New Brunswick</string>
      </Locations>
    </FeastItem>
    <FeastItem>
      <Id>13</Id>
      <Name>Canned Beans</Name>
      <Chance>10</Chance>
      <Locations>
        <string>all</string>
      </Locations>
    </FeastItem>
    <FeastItem>
      <Id>14</Id>
      <Name>Bottled Water</Name>
      <Chance>10</Chance>
      <Locations>
        <string>all</string>
      </Locations>
    </FeastItem>
    <FeastItem>
      <Id>10</Id>
      <Name>Police Vest</Name>
      <Chance>10</Chance>
      <Locations>
        <string>Ontario</string>
        <string>New Brunswick</string>
      </Locations>
    </FeastItem>
    <FeastItem>
      <Id>251</Id>
      <Name>White Travelpack</Name>
      <Chance>10</Chance>
      <Locations>
        <string>Ontario</string>
        <string>New Brunswick</string>
      </Locations>
    </FeastItem>
    <FeastItem>
      <Id>223</Id>
      <Name>Police Top</Name>
      <Chance>10</Chance>
      <Locations>
        <string>Ontario</string>
        <string>New Brunswick</string>
      </Locations>
    </FeastItem>
    <FeastItem>
      <Id>224</Id>
      <Name>Police Bottom</Name>
      <Chance>10</Chance>
      <Locations>
        <string>Ontario</string>
        <string>New Brunswick</string>
      </Locations>
    </FeastItem>
    <FeastItem>
      <Id>366</Id>
      <Name>Maple Crate</Name>
      <Chance>10</Chance>
      <Locations>
        <string>Ontario</string>
        <string>New Brunswick</string>
      </Locations>
    </FeastItem>
  </Items>
  <msgComingFeast>The feast is beginning at {0} in {1} minutes!</msgComingFeast>
  <msgNowFeast>The feast is now at {0}!</msgNowFeast>
</FeastConfiguration>
```

Most of these should be self explanatory.
## Configuration nodes ##
**enabled:** set to false if you really want to turn the feast off but not remove it from your server.
**minDropTime** and **maxDropTime** is the range of time between feast drops in seconds.  If you set both to the same number, it will be a set time.
**dropRadius** is how far from the location point they should randomly drop.  Default is 20 spaces.
**minItemsDrop** and **maxItemsDrop** is the range of number of items to be dropped each time.
**skyDrop** and **skyDropRadius**:  If set to true, the items will then spawn anywhere between 1 and the amount set on skyDropRadius in the air and fall to the ground.  Yes, they can fall on roofs.
**FeastItem** is a required piece.  It gives all the info the feast needs to drop a certain type.  Name doesn't have to be exact, but id does.  Chance is how many times to add that id into the pot.  10 is default for 10 types.  (10 items, 10 times each, makes it 10% chance each item will be this specific item.)
**Locations** is the most configurable.  Just put all, and that item will drop at all locations, or you can specify 1 or more locations for that item to drop.  So you can have items drop in one location and not another.  The names come from the map nodes, and they do have to be exact to take.
The two messages are what is sent to global chat before and during a feast.
All items are set to be removed when Unturned itself looks to sweep items that have been moved.  A message will be printed to the console that has the time and location of the next feast for logging purposes.

## Commands ##
This version of ZaupFeast features three new commands courtesy of nicholaiii <nicholainissen@gmail.com>. 
**ResetFeast**: Resets the Feast, resetting time and locations as according to your settings, as if it has just been run. *Alias: freset*
*Example usage*: `/resetfeast
**RunFeast**: Starts the Feast immediately at the  chosen location. *Alias: frun* 
*Example usage*: `/runfeast
**SetFeast**: Sets the desired location of the feast. Use the f parameter to force a location that is not included in the config. *Alias: fset*
*Example usage*: `/setfeast f SomeTown

*If you delete the configuration file, the system will recreate it on a restart but it will print a warning that it could not find it and disable the feast.  Just edit your config and restart the server. *

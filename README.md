# <Web Automation>

An C# console application automates the process of filling a flighting form for Allegiant Airlines.
   Base on the options provide it will calculate the total mount of the flight.
   
   It finds elements in the DOM (Document Object Model) of the html site and executre a behavior
   using C#.Net.
   
   The program has a set of predifined value for the user. There is not user interaction. It is for testing purposes.
   All value of the user are located at the beginning of each module.
   
   Executables are located under:
   directories: bin\Release\
   "web_automation\web_automation\bin\Release\netcoreapp2.2\"
   
   for
   win10 (x64), mac-10.11 (x64), linux (x64)

Submitted by: <Raul Rivero Rubio>

## User Stories

The following **required** functionality to proceed is complete:

* [X] Connects the WebDriver (Chrome)
* [X] Navigages to landing page
* [X] Closes the modal pop up.
* [X] Locates, clicks and writes the departure and destination area.
* [X] Locates, clicks the data for both departure date and return date using the calender calender
* [X] Locates, clicks Submit button.
* [X] Locates, clicks Continue button.

The following **additional** features are implemented:
* [ ] Locates, clicks and writes radio button for both roundtrip and oneway.
* [X] Locates, clicks the data write adult and child count for the attended flight.
* [X] Locates, clicks the data for the selected children.
* [ ] Locates, clicks radio button waiver when the child is under 2 years old.
* [X] Locates, clicks bonus or extra services.
* [X] Locates, clicks rental automobile specific Rental agency.
* [ ] Locates, clicks rental automobile specific Rental type.

## Video Walkthrough

Here's a walkthrough of implemented user stories:

<img src='web-automation-on-windows.gif' title='Web Automation for Aliante Airline on Windows' alt='Web Automation for Aliante Airline on Windows' />

## Notes

Describe any challenges encountered while building the app.
automate clickable calenders.
porting executable to different OS.

## License

Copyright 2019 <Raul Rivero Rubio>

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.

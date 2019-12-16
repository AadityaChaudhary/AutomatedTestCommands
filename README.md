# AutomatedTestCommands
A program that interprets simple commands from text files and runs corresponding actions in a web-testing environment.

### ENSURE YOU ENTER YOUR PATH TO YOUR CHROMEDRIVER IN driver.cs, in the constructor.

## arguments:
pass it the directory to where your "automated test folder" will be. This folder must have a results sub-folder 
and a tests sub-folder. If either does not exist, it will be added. Then fill the tests folder with your tests text files.

## commands: 
  
  1. your first word on each line should be the actual command. The available commands are:
  - OPEN
  - CLOSE
  - TYPE
  - CLICK
  - WAITFOR
  - CHECKVIS
  
  2. Each Command has a certain set of parameters.
  - OPEN only has 1 parameter, and thats the link you want to open. Please ensure that OPEN is only called on the first line
  - CLOSE doesn't have any parameters, and will close the browser and end that test
  - TYPE has 3 parameters. The first is how you are passing the input html object, the available options are ID, XPATH, and LINK.
  The next parameter is the object, passed in the form specified by the first parameter. The last parameter is text you wish to enter
  - CLICK, has 2 parameters. The parameters are the same as the TYPE parameters, except you cannot pass it any text to enter, as it will not enter any text, just click on the object
  - WAITFOR, has the same parameters as CLICK, and simply waits for the html object to become visible and interactable. It does not have a timeout feature just yet, so it will block the script, should the object never become enabled.
  - CHECKVIS has the same parameters as CLICK and WAITFOR, and returns whether the object is visible
  
  3. General info:
  - expected values (like commands, or html object descriptions, like XPATH, ID, etc) should be captialized and seperated by spaces.
  - unexpected values, like the url to go to, or the html objects id, should be surrounded by quotes (so that spaces can be included in these values)
  - results will be written to rootfoler/results/testname/timedatestamp.TXT
  ## example: 
  
  ```
  OPEN "http://somewebsite.com"
  
  WAITFOR ID "usernameInputID"
  TYPE ID "usernameInputID" "myusername"
  TYPE XPATH "//div/div/form/div/input" "my pass word "
  
  CLICK LINK "LOGIN >>"
  
  CLOSE

  ```
  

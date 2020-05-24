@ECHO OFF
FOR /L %%i IN (1,1,30) DO (
  START ExampleTCPClient.exe
  TIMEOUT /T 0.2
)
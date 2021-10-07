Start-Process pwsh { -c cd ./FunctionApp1 && func start -p 7071 }
Start-Process pwsh { -c cd ./FunctionApp2 && func start -p 7072 }
Start-Process pwsh { -c cd ./FunctionApp3 && func start -p 7073 }

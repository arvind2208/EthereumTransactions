# EthereumTransactions Api

EthereumTransactions Api is a .net core api that hosts services for ethereum transactions. The api currently has one endpoint to search transasctions by address and block

## Build

Run the following to build solution
CD into root folder
```
    dotnet build
```

## Test

Run both unit and integration tests by running the below command
CD into root folder
```
    dotnet test
```

## Run

```
    dotnet run --project .\EthereumTransactions\EthereumTransactions.csproj
```

## How to make the call
Host the api by running the above command and fire the following request and make sure you get the below response

```
    curl -v -L "http://localhost:5000/api/transactions?blockNumber=9148873&address=0xc55eddadeeb47fcde0b3b6f25bd47d745ba7e7fa"
```

### Sample payload
```
    {
		"transactions": [
			{
				"blockHash": "0x6dbde4b224013c46537231c548bd6ff8e2a2c927c435993d351866d505c523f1",
				"blockNumber": 9148873,
				"gas": 0.000000000000021,
				"hash": "0x1fd509bc8a1f26351400f4ca206dbe7b8ebb8dcf3925ddf7201e8764e1bd3ea3",
				"from": "0xc55eddadeeb47fcde0b3b6f25bd47d745ba7e7fa",
				"to": "0x59422595656a6b7c8917625607934d0e11fa0c40",
				"value": 80
			},
			{
				"blockHash": "0x6dbde4b224013c46537231c548bd6ff8e2a2c927c435993d351866d505c523f1",
				"blockNumber": 9148873,
				"gas": 0.000000000000021,
				"hash": "0xfcbbca93ff416601e5be95838fcfa2c534c48027b10172c12bf069784a4ec634",
				"from": "0xc55eddadeeb47fcde0b3b6f25bd47d745ba7e7fa",
				"to": "0x15776a03ef5fdf2a54a1b3a991c1641d0bfa39e7",
				"value": 17.4
			}
		]
	}
```

## Swagger link
Swagger link

```
    http://localhost:5000/swagger/index.html
```




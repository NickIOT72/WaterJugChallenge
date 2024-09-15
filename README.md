
# Water Jug Challenge

This program consist in obtein the steps with the best solution (shortest number of steps) to fill a bucket according the reference set on the input for the program

# How it works

    1. User sends information as JSON object of variables : x_capacity , y_capacity , z_amount_wanted.
    2. Server receive object.
    3. Server if json object is already saved on cache memory. In that case, response with the solution for that input.
    4. Server verify if :
        * parameters are higher than 0
        * parameters are even
        * x_capacity is higher than 2
        * z_amount_wanted is lower than x_capacity or z_amount_wanted is higher than y_capacity
        In case one of them happens, response with the correponding error.
    5. Start loop to fill Bucket Y until arrives to z_amount_wanted:
        5.1. for the first iteration => verify if x_capacity or y_capacity is closer to z_amount_wanted, to fill Bucket X or Bucket Y. Select with which Bucket start filling process
        NOTE: Bucket X have a caapcity of 2 gallons and Bucket Y the capacity of jug Y (y_capacity)
        5.2. Following iterations =>
            5.2.1 if selected bucket was Bucket X:
                * if Bucket X is Empty, then Fill it with 2 gallons. Generates step with message "Fill Bucket X"
                * if Bucket X is Filled, then transfer 2 gallons to Bucket Y from Bucket X ( Bucket X now empty). Generates step with message "Transfer from bucket X to Y"
            5.2.2. if selected bucket was Bucket Y:
                * if Bucket X is Empty, then transfer 2 gallons from Bucket Y to Bucket X. Bucket Y is reduced bby 2 gallons. Generates step with message "Transfer from bucket Y to X"
                * if Bucket X is Filled, then empty Bucket X. Generates step with message "Empty bucket X"

# Prerequirements

* Visual Studio 2022 or higher
* .NET Core 8.0

# API endpoints

These endpoints allow you to handle Stripe subscriptions for Publish and Analyze.

## GET
`get all solutions` [/api/bucket/](#get-api-bucket)

## POST
`post input data` [/api/bucket/](#get-api-bucket)

### GET /api/bucket/
Obtain all solutions saved on cache memory.

**Response Parameters**

|          Name | Required |  Type   |   Description |
| -------------:|:--------:|:-------|--------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
|`solution` | required | List  | Contains the number of steps generated for the best solution |
|`steps` | required | List  | Information of each iteration executed on program to optimize outcome solution
|`step` | required | int  | number of step
|`bucket_x` | required | int  | outcome gallons for bucket X
|`bucket_y` | required | int  | outcome gallons for bucket Y
|`action_msg` | required | string  | Shows the information about the action executed for the step ( Fill, Empty, and Transfer (between the two jugs) )
|`status` | required | string  | Verify if solution was found or not
|`bucket` | required | List  | Input information used to generate solution
|`x_capacity` | required | int  | capacity 
|`bucket` | required | List  | Input information used to generate solution
|`x_capacity` | required | int  | Capacity of gallons for jug X                                                     |
|`y_capacity` | required | int  | Capacity of gallons for jug Y
|`z_amount_wanted` | required | int  | Reference for outcome gallons for process

**Response JSON format**

```
[
  {
    "solution": {
      "steps": [
        {
          "step": 1,
          "bucket_x": 0,
          "bucket_y": 100,
          "action_msg": "Fill bucket y",
          "status": ""
        },
        {
          "step": 2,
          "bucket_x": 2,
          "bucket_y": 98,
          "action_msg": "Fill bucket x",
          "status": ""
        },
        {
          "step": 3,
          "bucket_x": 0,
          "bucket_y": 98,
          "action_msg": "Empty bucket x",
          "status": ""
        },
        {
          "step": 4,
          "bucket_x": 2,
          "bucket_y": 96,
          "action_msg": "Fill bucket x",
          "status": "SOLVED"
        }
      ]
    },
    "bucket": {
      "x_capacity": 2,
      "y_capacity": 100,
      "z_amount_wanted": 96
    }
  }
]
```

### POST /api/bucket/
Send a JSON object that contains the capacity for jugs X and Y and the desired outcome of Z gallons

**Input Parameters**

|          Name | Required |  Type   |   Description |
| -------------:|:--------:|:-------:|--------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
|`x_capacity` | required | int  | Capacity of gallons for jug X                                                     |
|`y_capacity` | required | int  | Capacity of gallons for jug Y
|`z_amount_wanted` | required | int  | Reference for outcome gallons for process

**Input JSON format**

```
// Payload
{
    "x_capacity": 2,
    "y_capacity": 10,
    "z_amount_wanted": 4,
}
```

**Response Parameters**

|          Name | Required |  Type   |   Description |
| -------------:|:--------:|:-------: --------------------------------------------------------------------------------------------------------------------------------------------------------------------- |                                                   |
|`steps` | required | List  | Information of each iteration executed on program to optimize outcome solution
|`step` | required | int  | number of step
|`bucket_x` | required | int  | outcome gallons for bucket X
|`bucket_y` | required | int  | outcome gallons for bucket Y
|`action_msg` | required | string  | Shows the information about the action executed for the step ( Fill, Empty, and Transfer (between the two jugs) )
|`status` | required | string  | Verify if solution was found or not

**Response JSON format**

```
// Payload
{
  "steps": [
    {
      "step": 1,
      "bucket_x": 2,
      "bucket_y": 0,
      "action_msg": "Fill bucket x",
      "status": ""
    },
    {
      "step": 2,
      "bucket_x": 0,
      "bucket_y": 2,
      "action_msg": "Transfer from bucket x to bucket y",
      "status": ""
    },
    {
      "step": 3,
      "bucket_x": 2,
      "bucket_y": 2,
      "action_msg": "Fill bucket x",
      "status": ""
    },
    {
      "step": 4,
      "bucket_x": 0,
      "bucket_y": 4,
      "action_msg": "Transfer from bucket x to bucket y",
      "status": "SOLVED"
    }
  ]
}
```

## How To Run

* Open solution in Visual Studio 
* Run the application.

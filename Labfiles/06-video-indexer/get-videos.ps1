$account_id="17211d62-8e60-4eb3-b5cf-8b6667f7450f"
$api_key="YOUR_API_KEY"

# Azure resource location
$location="trial"

# Call the AccessToken method with the API key in the header to get an access token
$token = Invoke-RestMethod -Method "Get" -Uri "https://api.videoindexer.ai/auth/$location/Accounts/$account_id/AccessToken" -Headers @{'Ocp-Apim-Subscription-Key' = $api_key}

# Use the access token to make an authenticated call to the Videos method to get a list of videos in the account
Invoke-RestMethod -Method "Get" -Uri "https://api.videoindexer.ai/$location/Accounts/$account_id/Videos?accessToken=$token" | ConvertTo-Json -Depth 6

param(
    [Parameter(Mandatory=$true)]
    [string] $subscriptionId,
    [Parameter(Mandatory=$true)]
    [string] $resourceGroupName,
    [Parameter(Mandatory=$true)]
    [string] $appName
)
   az ad sp create-for-rbac --name $appName --role contributor `
                            --scopes "/subscriptions/$subscriptionId/resourceGroups/$resourceGroupName" `
                            --sdk-auth
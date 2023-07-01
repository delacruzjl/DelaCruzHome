@allowed([
  'westus2'
  'centralus'
  'eastus2'
  'westeurope'
  'eastasia'
])
param location string
param name string
param github_repo string
param sku string = 'Free'
param main_branch string = 'main' // Could be master


resource stapp 'Microsoft.Web/staticSites@2021-02-01' = {
  name: name
  location: location
  sku: {
    name: sku
    tier: sku
  }
  properties: {
    #disable-next-line BCP073
    provider: 'GitHub'
    repositoryUrl: github_repo
    branch: main_branch
    stagingEnvironmentPolicy: 'Enabled'
    allowConfigFileUpdates: true   
  }
}

output stappId string = stapp.id

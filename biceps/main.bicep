// az deployment sub create -n website --location eastus2 --template-file .\main.bicep --parameters .\parameters-dlch.json

targetScope = 'subscription'

param location string = 'eastus2'
param resource_group_name string
param stapp_name string
param app_config_name string

param github_repo_uri string
// param custom_domain_name string = ''

@secure()
param sendGrid_Key string
@secure()
param sendgrid_newsletterId string
@secure()
param sendgrid_templateId string
@secure()
param sentry_dsn string
param sendgrid_destination_email string 
param owner_email_address string
param owner_name string
param sendgrid_groupId string
param email_subject_line string

module rg 'resource-group.bicep' = {
  name: 'rg'
  params: {
    location: location
    name: resource_group_name
  }
}

module appcs 'app-configuration.bicep' = {
  scope: resourceGroup(resource_group_name)
  name: 'appcs'
  params: {
    name: app_config_name
    location: location
  }
  dependsOn: [
    rg
  ]
}

module stapp 'static-webapp.bicep' = {
  name: 'stapp'
  scope: resourceGroup(resource_group_name)
  params: {
    location: location
    name: stapp_name
    github_repo: github_repo_uri
  }
  dependsOn: [
    rg
  ]
}

module stappconfig 'static-webapp-config.bicep' = {
  scope: resourceGroup(resource_group_name)
  name: 'stappconfig'
  params: {
    app_config_name: app_config_name
    stapp_name: stapp_name
  }
  dependsOn: [
    rg, appcs, stapp
  ]
}

module settings 'app-settings-array.bicep' = {
  name: 'settings'
  params: {
    resource_group_name: resource_group_name
    app_config_name: app_config_name
    email_subject_line: email_subject_line
    owner_email_address: owner_email_address
    owner_name: owner_name
    sendgrid_destination_email: sendgrid_destination_email
    sendgrid_groupId: sendgrid_groupId
    sendGrid_Key: sendGrid_Key
    sendgrid_newsletterId: sendgrid_newsletterId
    sendgrid_templateId: sendgrid_templateId
    sentry_dsn: sentry_dsn
  }
  dependsOn: [
    rg, appcs
  ]
}



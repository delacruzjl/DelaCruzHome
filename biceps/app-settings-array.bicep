param resource_group_name string
param app_config_name string
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

var sendgrid_prefix = 'SendGrid:'

var allSettings = [
  {
    key: 'AzureWebJobsSendGridApiKey'
		label: null
		value: sendGrid_Key
		content_type: ''
		tags: {}
  }
{
  key: '${sendgrid_prefix}EmailAddress'
  label: null
  value: owner_email_address
  content_type: ''
  tags: {}
}
{
  key: '${sendgrid_prefix}NewsletterListId'
  label: null
  value: sendgrid_newsletterId
  content_type: ''
  tags: {}
}
{
  key: '${sendgrid_prefix}SenderName'
  label: null
  value: owner_name
  content_type: ''
  tags: {}
}
{
  key: '${sendgrid_prefix}SubjectLine'
  label: null
  value: email_subject_line
  content_type: ''
  tags: {}
}
{
  key: '${sendgrid_prefix}SuppressionGroupId'
  label: null
  value: sendgrid_groupId
  content_type: ''
  tags: {}
}
{
  key: '${sendgrid_prefix}TemplateId'
  label: null
  value: sendgrid_templateId
  content_type: ''
  tags: {}
}
{
  key: '${sendgrid_prefix}WebsiteAdminEmail'
  label: null
  value: sendgrid_destination_email
  content_type: ''
  tags: {}
}
{
  key: 'sentry.dsn'
  label: null
  value: sentry_dsn
  content_type: ''
  tags: {}
}]

resource appcs 'Microsoft.AppConfiguration/configurationStores@2021-03-01-preview' existing = {
  name: app_config_name
}

module appConfigKeys 'app-config-setting.bicep' =[for (item, idx) in allSettings: {
  scope: resourceGroup(resource_group_name)
  name: 'appConfigKeys-${idx}'
  params: {
    configName: appcs.name
    itemKey: item.key
    itemValue: item.value
    itemContentType: item.content_type
    itemLabel: item.label
    itemTag: item.tags
  }
}]

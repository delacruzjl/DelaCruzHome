targetScope = 'subscription'

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
param swaggerVersion string = '1.0.0'
param swaggerTitle string = 'Jose\'s SendGrid Newsletter Subscription API'
param swaggerDescription string = 'API designed by [http://delacruzhome.com](http://delacruzhome.com).'
param swaggerTermsOfService string = 'https://github.com/delacruzjl'
param swaggerContactName string = 'Jose De La Cruz'
param swaggerContactEmail string = 'jose@delacruzhome.com'
param swaggerContactUrl string = 'https://github.com/delacruzjl'
param swaggerLicenseName string = 'MIT'
param swaggerLicenseUrl string = 'http://opensource.org/licenses/MIT'

var sendgrid_prefix = 'SendGrid:'
var swaggerInfo_prefix = 'ApiSwaggerInfo:'
var swaggerInfo_apiContact_prefix = '${swaggerInfo_prefix}ApiContact:'
var swaggerInfo_apiLicense_prefix = '${swaggerInfo_prefix}ApiLicense:'

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
}

{
  key: '${swaggerInfo_prefix}Version'
  label: null
  value: swaggerVersion
  content_type: ''
  tags: {}
}
{
  key: '${swaggerInfo_prefix}Title'
  label: null
  value: swaggerTitle
  content_type: ''
  tags: {}
}
{
  key: '${swaggerInfo_prefix}Description'
  label: null
  value: swaggerDescription
  content_type: ''
  tags: {}
}
{
  key: '${swaggerInfo_prefix}TermsOfService'
  label: null
  value: swaggerTermsOfService
  content_type: ''
  tags: {}
}
{
  key: '${swaggerInfo_apiContact_prefix}Name'
  label: null
  value: swaggerContactName
  content_type: ''
  tags: {}
}
{
  key: '${swaggerInfo_apiContact_prefix}Email'
  label: null
  value: swaggerContactEmail
  content_type: ''
  tags: {}
}
{
  key: '${swaggerInfo_apiContact_prefix}Url'
  label: null
  value: swaggerContactUrl
  content_type: ''
  tags: {}
}
{
  key: '${swaggerInfo_apiLicense_prefix}Name'
  label: null
  value: swaggerLicenseName
  content_type: ''
  tags: {}
}
{
  key: '${swaggerInfo_apiLicense_prefix}Url'
  label: null
  value: swaggerLicenseUrl
  content_type: ''
  tags: {}
}
]

resource appcs 'Microsoft.AppConfiguration/configurationStores@2021-03-01-preview' existing = {
  scope: resourceGroup(resource_group_name)  
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

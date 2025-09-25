CONTAINER_APP_NAME = "$(CONTAINER_APP_NAME)"
RESOURCE_GROUP_NAME_CONTAINER_APP = "$(RESOURCE_GROUP_NAME_CONTAINER_APP)"
RESOURCE_GROUP_NAME_CONTAINER_ENV = "$(RESOURCE_GROUP_NAME_CONTAINER_ENV)"
CONTAINER_APP_ENV_NAME = "$(CONTAINER_APP_ENV_NAME)"
  
REGISTRY_SERVER = {
  SERVER = "$(REGISTRY_SERVER)",
  USERNAME = "$(REGISTRY_USERNAME)"
  PASSWORD_SECRET_NAME = "$(REGISTRY_PASSWORD_SECRET_NAME)"
  REPOSITORY = "$(REGISTRY_REPOSITORY)"
  TAG = "$(REGISTRY_TAG)"
  PASSWORD = "$(REGISTRY_PASSWORD)"
}

CLIENT_ID  = "$(CLIENT_ID)"
TENNANT_ID =  "$(TENNANT_ID)"
CLIENT_SECRET  = "$(CLIENT_SECRET)"
SUBSCRIPTION_ID  = "$(SUBSCRIPTION_ID)"

CONTAINER_ENVS = [ {
      name        = "GiproDatabases__ConnectionString"
      secret_name = "connectionstring"
      value       = "no aplica"
    },
    {
      name        = "GiproDatabases__DatabaseNameCollection__Mundial"
      secret_name = "database-mundial-name"
      value       = "no aplica"
    },
    {
      name        = "Serilog__WriteTo__1__Args__serverUrl"
      secret_name = "seq-server-url"
      value       = "no aplica"
    },
    {
      name        = "BasicGiproDatabases__ConnectionString"
      secret_name = "basics-connection-string"
      value       = "no aplica"
    },
    {
      name        = "BasicGiproDatabases__DatabaseNameCollection__Mundial"
      secret_name = "database-basics-name"
      value       = "no aplica"
    } ]

SECRETS = [ {
  name = "connectionstring"
  value = "$(CONNECTIONSTRING-DB)"
  identity = null
  key_vault_secret_id = null
},
{
  name = "database-mundial-name"
  value = "$(DATABASE-MUNDIAL-NAME)"
  identity = null
  key_vault_secret_id = null
},
{
  name = "seq-server-url"
  value = "$(SERVER-SEQ-LOG)"
  identity = null
key_vault_secret_id = null
},
{
  name = "basics-connection-string"
  value = "$(CONNECTIONSTRING-DB-BASIC)"
  identity = null
key_vault_secret_id = null
},
{
  name = "database-basics-name"
  value = "$(DATABASE-BASIC-NAME)"
  identity = null
key_vault_secret_id = null
} ]
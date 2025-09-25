variable "CLIENT_ID" {
  description = "client id"
  type        = string
}

variable "TENNANT_ID" {
  description = "client id"
  type        = string
}

variable "SUBSCRIPTION_ID" {
  description = "client id"
  type        = string
}

variable "CLIENT_SECRET" {
  description = "client secret"
  type        = string
}

variable "RESOURCE_GROUP_NAME_CONTAINER_APP" {
  description = "Name of resource group"
  type        = string
}

variable "RESOURCE_GROUP_NAME_CONTAINER_ENV" {
  description = "Name of resource group"
  type        = string
}

variable "CONTAINER_APP_NAME" {
  type = string
  description = "container app name"
}

variable "CONTAINER_APP_ENV_NAME" {
  type = string
  description = "container env name"
}

variable "REGISTRY_SERVER" {
  type = object({
    SERVER = string
    USERNAME = string
    PASSWORD_SECRET_NAME =string
    PASSWORD = string
    REPOSITORY = string
    TAG = string
  })
}

variable "CONTAINER_ENVS" {
  type = list(object({
    name        = string
    secret_name = string
    value       = string
  }))
  
}

variable "SECRETS" {
  type = list(object({
    name     = string
    value    = string
    identity = string
    key_vault_secret_id = string
  }))

}
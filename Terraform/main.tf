
terraform {
  backend "azurerm" {
    resource_group_name  = "gipro_transversal_int_rg"  # Can be passed via `-backend-config=`"resource_group_name=<resource group name>"` in the `init` command.
    storage_account_name = "giprosaint"                      # Can be passed via `-backend-config=`"storage_account_name=<storage account name>"` in the `init` command.
    container_name       = "tfstate"                       # Can be passed via `-backend-config=`"container_name=<container name>"` in the `init` command.
    key                  = "ruleengine.terraform.tfstate"        # Can be passed via `-backend-config=`"key=<blob key name>"` in the `init` command.
  }
}

data "azurerm_container_app_environment" "container-environment" {
  name               = var.CONTAINER_APP_ENV_NAME
  resource_group_name = var.RESOURCE_GROUP_NAME_CONTAINER_ENV
}

output "container-app-env" {
  value = data.azurerm_container_app_environment.container-environment.id
}


resource "azurerm_container_app" "container_app_name" {
  name                         = var.CONTAINER_APP_NAME
  container_app_environment_id = data.azurerm_container_app_environment.container-environment.id
  resource_group_name          = var.RESOURCE_GROUP_NAME_CONTAINER_APP
  revision_mode                = "Single"


  template {
    container {
      name   = var.CONTAINER_APP_NAME
      image  = "${var.REGISTRY_SERVER.SERVER}/${var.REGISTRY_SERVER.REPOSITORY}:${var.REGISTRY_SERVER.TAG}"
      cpu    = 0.25
      memory = "0.5Gi"
      
     dynamic "env" {
       for_each = var.CONTAINER_ENVS
       content {
        name        = env.value.name
        secret_name = env.value.secret_name
        value       = env.value.value
       }
      }
    }
  }

  registry {
    server = var.REGISTRY_SERVER.SERVER
    username = var.REGISTRY_SERVER.USERNAME
    password_secret_name = var.REGISTRY_SERVER.PASSWORD_SECRET_NAME
  }

  ingress {
    target_port = 8080
    external_enabled = true
    traffic_weight {
      latest_revision = true
      percentage = 100
    }
  }

  secret {
    name = var.REGISTRY_SERVER.PASSWORD_SECRET_NAME
    value = var.REGISTRY_SERVER.PASSWORD
  }

  dynamic "secret" {
    for_each = var.SECRETS
      content {
        name  = secret.value.name
        value = secret.value.value
       }
  }
  # secret {
  #   name = "var-secret1"
  #   value = var.registry_server.password
  #   # identity = data.azurerm_user_assigned_identity.gipro-identities.id
  #   # key_vault_secret_id = "https://gipro-dev-kv.vault.azure.net/secrets/gipro-mundial-db/0a4fa14b91e44043a518f27b585cbc99"
  # }

  # secret {
  #   name = "var-secret2"
  #   value = var.registry_server.password
  #   # identity = data.azurerm_user_assigned_identity.gipro-identities.id
  #   # key_vault_secret_id = "https://gipro-dev-kv.vault.azure.net/secrets/gipro-mundial-db/0a4fa14b91e44043a518f27b585cbc99"
  # }
  
}
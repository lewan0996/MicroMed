# Keycloak realm export

In order to export a realm to a JSON file, run the following command from the `docker-compose` folder:

```bash
docker run --rm \
  -v $(pwd)/containers/keycloak:/opt/keycloak/data:Z \
  quay.io/keycloak/keycloak:latest \
  export \
  --realm micromed \
  --dir /opt/keycloak/data/import \
  --users realm_file
```
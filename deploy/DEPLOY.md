# Deploy Astibosoft.Api to a VPS

Same infra pattern as FitCore: external Docker networks `edge` + `database`, shared Postgres, host nginx on the edge.

## Naming / conflicts

| Risk | Safe if… |
|------|----------|
| Docker DNS on `edge` | Service is **`astibosoft-api`**, not `api`. |
| Public URL | Own host `astibosoft.com` (Infra nginx). |
| Database | `POSTGRES_DB=astibosoft`. Same Postgres instance is fine. |
| Compose project | `astibosoft-vps` — no clash with `fitcore-vps` / others. |

Nginx must proxy to **`http://astibosoft-api:8080`** (see Infra `astibosoft-locations.conf`).

## First deploy

```bash
cd ~/Astibosoft.Api/deploy
cp .env.example .env
nano .env
chmod +x docker-init.sh
docker compose -f compose.vps.yaml --env-file .env up -d --build
```

Migrations run on API startup (`Database__AutoMigrate=true`).

## Nginx + static site (Infra repo)

Edge config is in **Infra** (`nginx/conf.d/astibosoft.conf` + `snippets/astibosoft-locations.conf`):

- Host: `astibosoft.com` / `www.astibosoft.com`
- Static: `ASTIBOSOFT_DIST` → `/usr/share/nginx/astibosoft` (Next.js `out/`)
- API: `/api/` → `http://astibosoft-api:8080`

On the VPS, after DNS points at the box:

```bash
sudo docker compose -f ~/Infra/compose.yaml stop nginx
sudo certbot certonly --standalone -d astibosoft.com -d www.astibosoft.com
sudo docker compose -f ~/Infra/compose.yaml up -d
```

Set `ASTIBOSOFT_DIST` in Infra `.env` to the built static export path, then recreate nginx.

## Useful commands

```bash
cd ~/Astibosoft.Api/deploy
docker compose -f compose.vps.yaml --env-file .env ps
docker compose -f compose.vps.yaml --env-file .env logs -f astibosoft-api
docker compose -f compose.vps.yaml --env-file .env up -d --build
docker compose -f compose.vps.yaml --env-file .env down
```

## Update

```bash
cd ~/Astibosoft.Api && git pull
cd deploy && docker compose -f compose.vps.yaml --env-file .env up -d --build
```

## Security

- Never commit `.env`
- Prefer HTTPS on `astibosoft.com` before opening the contact form publicly

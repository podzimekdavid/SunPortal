cd .
echo "Deploying changes..."

git pull

docker-compose down
docker-compose up -d -- build

echo "Deployed!"
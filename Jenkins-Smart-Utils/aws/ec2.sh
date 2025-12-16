# Chmod 400
chmod 400 microservicesapp.pem

ssh -i microservicesapp.pem ec2-user@public-ip-address

docker version

docker pull hashicorp/consul:1.16

docker run -d \
  --name consul-server \
  -p 8500:8500 \
  -p 8300:8300 \
  -p 8301:8301 \
  hashicorp/consul:1.16 agent \
  -server \
  -bootstrap \
  -ui \
  -client=0.0.0.0

  docker ps

  # Ensure the inbound port is open for 8500
 
 docker exec consul-server netstat -tulpn

 curl http://localhost:8500


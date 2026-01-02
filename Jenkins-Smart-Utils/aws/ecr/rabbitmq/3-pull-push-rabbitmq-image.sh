docker pull rabbitmq:3.13.0-management

docker tag rabbitmq:3.13.0-management 395109667422.dkr.ecr.us-east-1.amazonaws.com/rabbitmq-repo:3.13.0-management

docker push 395109667422.dkr.ecr.us-east-1.amazonaws.com/rabbitmq-repo:3.13.0-management
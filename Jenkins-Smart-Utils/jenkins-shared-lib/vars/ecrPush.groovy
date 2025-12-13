def call(String localImage, String ecrImage){
    withAWS(credentials:'aws-creds',region:'us-east-1'){
                sh """
                AWS_ACCOUNT_ID=\$(aws sts get-caller-identity --query Account --output text)

                # Login to ECR
                aws ecr get-login-password --region us-east-1 | docker login --username AWS --password-stdin \${AWS_ACCOUNT_ID}.dkr.ecr.us-east-1.amazonaws.com
                
                # Tag the local image to match ECR repo
                docker tag ${localImage} ${ecrImage}
                
                # Push to ECR
                docker push ${ecrImage}
                """             
        }

}
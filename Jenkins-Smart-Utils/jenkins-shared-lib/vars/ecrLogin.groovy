def call(String awsRegion){
    sh """
        aws ecr get-login-password --region ${awsRegion} \
        | docker login --username AWS --password-stdin \
        $(aws sts get-caller-identity --query 'Account' --output text).dkr.ecr.${awsRegion}.amazonaws.com
    """
}
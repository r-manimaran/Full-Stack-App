def call(String cluster, String service){
    sh """
        aws ecs update-service \
        --cluster ${cluster} \
        --service ${service} \
        --force-new-deployment
    """
}
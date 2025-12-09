def call(String name, int port){
    sh """
        curl --request PUT \
        --data '{"Name": "${name}", "Port": ${port}}' \
         http://localhost:8500/v1/agent/service/register
       """
}
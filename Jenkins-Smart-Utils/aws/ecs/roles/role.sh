# Generate trust policy document
cat > /tmp/ecs-task-exec-trust-policy.json <<EOF
{
  "Version": "2012-10-17",
  "Statement": [
    {
      "Effect": "Allow",
      "Principal": {
        "Service": "ecs-tasks.amazonaws.com"
      },
      "Action": "sts:AssumeRole"
    }
  ]
}
EOF

# create role
TASK_EXEC_ROLE_ARN=$(aws iam create-role \
  --role-name "${PROJECT_NAME}-ecs-task-execution-role" \
  --assume-role-policy-document file:///tmp/ecs-task-exec-trust-policy.json \
  --tags Key=Project, Values=${PROJECT_NAME} \
  --query 'Role.{ARN:Arn}' \
  --output text)

echo "Task execution role ARN: $TASK_EXEC_ROLE_ARN"

# Attach policies to the role
aws iam attach-role-policy \
    --role-name "${PROJECT_NAME}-ecs-task-execution-role" \
    --policy-arn "arn:aws:iam::aws:policy/service-role/AmazonECSTaskExecutionRolePolicy"

# Create task role (for application-specific permissions)
TASK_ROLE_ARBN=$(aws iam create-role \
  --role-name "${PROJECT_NAME}-ecs-task-role" \
  --assume-role-policy-document file:///tmp/ecs-task-exec-trust-policy.json \
  --tags Key=Project,Values=${PROJECT_NAME} \
  --query 'Role.{ARN:Arn}' \
  --output text)

  echo "Task role ARN: $TASK_ROLE_ARBN"

# Attach policies to the task role
# If the application needs any aws services to be accessed, attach the respective policies here
# For example, if the application needs to access S3, attach the AmazonS3ReadOnly
aws iam attach-role-policy \
    --role-name "${PROJECT_NAME}-ecs-task-role" \
    --policy-arn "arn:aws:iam::aws:policy/AmazonS3ReadOnlyAccess"
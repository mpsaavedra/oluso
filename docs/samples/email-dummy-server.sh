#!/usr/bin/bash
echo "Starting SMTP debug server"
python -m smtpd -n -c DebuggingServer localhost:1025

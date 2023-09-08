#!/bin/bash

#git log --oneline | cut -c 9-10000 |  grep -e ^feat: -e ^fix: -e ^ci: -e ^chore: -e ^docs: -e ^test: -e ^style: -e ^refactor: -e ^wip: >> change-log.txt
# include only important changes only
git log --oneline | cut -c 9-10000 |  grep -e ^feat: -e ^fix: >> change-log.txt

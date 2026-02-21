TODO

- 增加全过程日志拦截器，增加查看日志接口
- 增加注册，注销接口
- 增加修改密码接口
- 增加邮件接口
- 增加第三方登录

## 部署相关

~~~
sudo vim /etc/systemd/system/blunt-serve.service
~~~

~~~
[Unit]
Description=BluntServe .NET 10 Service
After=network.target

[Service]
WorkingDirectory=/var/www/blunt-serve
ExecStart=/usr/bin/dotnet /var/www/blunt-serve/BluntServe.dll
Restart=always
# 如果崩溃，10秒后重启
RestartSec=10
KillSignal=SIGINT
SyslogIdentifier=dotnet-bluntserve
User=root
# 关键：告诉程序使用生产环境配置
Environment=ASPNETCORE_ENVIRONMENT=Production
# 如果你想让外部通过 IP 访问，改为 0.0.0.0
Environment=ASPNETCORE_URLS=http://0.0.0.0:5000

[Install]
WantedBy=multi-user.target
~~~

~~~
## 自启动
sudo systemctl daemon-reload
sudo systemctl enable blunt-serve.service
sudo systemctl start blunt-serve.service

# 查看是否运行成功
sudo systemctl status blunt-serve.service
~~~

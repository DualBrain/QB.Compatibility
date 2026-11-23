' From the QBasic manual: https://qbasic.net/en/qb-manual/Statement/OPEN%20COM.htm
'Use this example for trouble shooting serial communications problems.
'Slow baud, hardware handshaking is ignored and buffers are enlarged.
OPEN "COM1:300,N,8,1,CD0,CS0,DS0,OP0,RS,TB2048,RB2048" FOR RANDOM AS #1

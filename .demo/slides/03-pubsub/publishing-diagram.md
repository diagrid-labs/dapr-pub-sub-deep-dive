---
theme: default
layout: default
---

```mermaid
---
title: Dapr Pub/Sub
config:
  theme: light
---
graph LR
    S[Sender Service]
    subgraph Broker
        T@{ shape: das, label: "Topic" }
    end
    R[Receiver Service]
    S -.->|Publish
    Message| T
    T -.->|Receive
    Message| R
```
---
theme: default
layout: default
---

## The transactional outbox pattern is a well known design pattern for sending notifications regarding changes in an application’s state. 

```mermaid
---
title: Outbox Pattern
config:
  theme: light
---
graph LR
    S[Sender]
    R[Receiver]
    ST[(StateStore)]
    subgraph Transaction
      subgraph Broker
      T@{ shape: das, label: "Topic" }
      end
        S -.->|2 Message| T
        S --->|1 Save| ST
    end
    T -.->|3 Message| R
```
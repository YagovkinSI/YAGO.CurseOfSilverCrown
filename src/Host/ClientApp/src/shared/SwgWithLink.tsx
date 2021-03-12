import { Button } from "@mui/material";
import React from "react";

interface SwgWithLinkProps {
  url: string;
  swgPath: string;
  alt: string;
}

const SwgWithLink: React.FC<SwgWithLinkProps> = (prop: SwgWithLinkProps) => {
  return (
    <Button variant="outlined" href={prop.url} target="_blank">
      <img style={{ maxWidth: '80%' }} src={prop.swgPath} alt={prop.alt} />
    </Button>
  )
}

export default SwgWithLink
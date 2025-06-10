import { useEffect } from "react";

const ExternalRedirect = ({ to }: { to: string }) => {
  
  useEffect(() => {
    window.location.href = to;
  }, [to]);

  return <div>Перенаправление...</div>;
};

export default ExternalRedirect;
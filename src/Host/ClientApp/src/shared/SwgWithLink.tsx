import React from "react";

interface SwgWithLinkProps {
  url: string;
  swgPath: string;
  alt: string;
}

const SwgWithLink: React.FC<SwgWithLinkProps> = (prop: SwgWithLinkProps) => {
  const renderImage = () => (
    <img 
      src={prop.swgPath} 
      alt={prop.alt} 
      className="max-w-[80%] h-auto object-contain"
    />
  );

  return (
    <a
      href={prop.url}
      target="_blank"
      rel="noopener noreferrer"
      className="
        inline-flex items-center justify-center
        px-4 py-2
        border border-bright/30 rounded-md
        bg-transparent
        text-bright
        hover:bg-bright/10 hover:border-bright/50
        transition-all duration-200
        focus:outline-none focus:ring-2 focus:ring-bright/50
        min-w-[64px] min-h-[36px]
      "
    >
      {renderImage()}
    </a>
  );
}

export default SwgWithLink;
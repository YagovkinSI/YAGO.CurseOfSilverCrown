const YagoAvatar: React.FC<{ name: string; size?: 'sm' | 'md' | 'lg' }> = ({ 
    name, 
    size = 'md' 
}) => {
    const sizeMap = {
        sm: 'w-8 h-8 text-xs',
        md: 'w-10 h-10 text-sm',
        lg: 'w-12 h-12 text-base',
    };

    const wordsOfname = name.split(' ');
    const symbolsOfName = wordsOfname.length == 1
        ? `${wordsOfname[0][0]}`
        : `${wordsOfname[0][0]}${wordsOfname[1][0]}`

    return (
        <div className={`
            rounded-full bg-bright/20 border border-bright/30 
            flex items-center justify-center text-light font-semibold
            ${sizeMap[size]}
        `}>
            {symbolsOfName}
        </div>
    );
};

export default YagoAvatar;
interface YagoAvatarProps {
    name: string;
    size?: 'sm' | 'md' | 'lg';
}

const YagoAvatar: React.FC<YagoAvatarProps> = ({ name, size = 'md' }) => {
    const stringToColor = (string: string) => {
        let hash = 0;
        let i;
        for (i = 0; i < string.length; i += 1) {
            hash = string.charCodeAt(i) + ((hash << 5) - hash);
        }
        let color = '#';
        for (i = 0; i < 3; i += 1) {
            const value = (hash >> (i * 8)) & 0xff;
            color += `00${value.toString(16)}`.slice(-2);
        }
        return color;
    };

    const getInitials = (name: string) => {
        const wordsOfName = name.trim().split(' ');
        if (wordsOfName.length == 1) {
            return wordsOfName[0][0].toUpperCase();
        }
        return `${wordsOfName[0][0]}${wordsOfName[1][0]}`.toUpperCase();
    };

    const getSizeClasses = () => {
        switch (size) {
            case 'sm':
                return 'w-8 h-8 text-xs';
            case 'lg':
                return 'w-12 h-12 text-lg';
            case 'md':
            default:
                return 'w-10 h-10 text-sm';
        }
    };

    const avatarColor = stringToColor(name);
    const initials = getInitials(name);
    const sizeClasses = getSizeClasses();

    return (
        <div
            className={`
                ${sizeClasses}
                rounded-full flex items-center justify-center
                font-medium text-white select-none
                transition-all duration-200
                hover:scale-105
            `}
            style={{ backgroundColor: avatarColor }}
            aria-label={`Аватар пользователя ${name}`}
        >
            {initials}
        </div>
    );
};

export default YagoAvatar;
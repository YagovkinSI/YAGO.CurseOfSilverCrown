import Avatar from '@mui/material/Avatar';

interface YagoAvatarProps{
    name: string
}

const YagoAvatar : React.FC<YagoAvatarProps> = ({ name }) => {

    const stringToColor =(string: string) => {
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
    }

    const stringAvatar = (name: string) => {
        const wordsOfname = name.split(' ');
        const symbolsOfName = wordsOfname.length == 1
            ? `${wordsOfname[0][0]}`
            : `${wordsOfname[0][0]}${wordsOfname[1][0]}`

        return {
            sx: {
                bgcolor: stringToColor(name),
            },
            children: symbolsOfName,
        };
    }


    return (
        <Avatar {...stringAvatar(name)} />
    );
}

export default YagoAvatar;
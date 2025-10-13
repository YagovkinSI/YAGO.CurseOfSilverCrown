import { Box, TextField } from "@mui/material";

interface YagoCardSContentInputFieldProps {
    name: string,
    handleChange: (e: React.ChangeEvent<HTMLInputElement>) => void,
    error: string
}

const YagoCardSContentInputField: React.FC<YagoCardSContentInputFieldProps> = ({ name, handleChange, error }) => {
    return (
        <Box mx={2} textAlign="center">
            <Box mb={2}>
                <TextField
                    fullWidth
                    label="Название колонии"
                    value={name}
                    onChange={handleChange}
                    error={!!error}
                    helperText={error}
                    inputProps={{
                        maxLength: 16,
                        pattern: '[a-zA-Zа-яА-Я0-9 -]{3,16}'
                    }}
                    sx={{ mb: 2 }} />
            </Box>
        </Box>
    );
}

export default YagoCardSContentInputField
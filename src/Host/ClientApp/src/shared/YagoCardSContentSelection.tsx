import { ArrowBack, ArrowForward } from "@mui/icons-material";
import { Box, IconButton, Typography } from "@mui/material";

interface YagoCardSContentSelectionProps {
    handlePrev: () => void,
    label: string,
    handleNext: () => void
}

const YagoCardSContentSelection: React.FC<YagoCardSContentSelectionProps> = ({ handlePrev, label, handleNext }) => {
    return (
        <Box display="flex" alignItems="center" justifyContent="space-between" mb='8px' >
            <IconButton onClick={handlePrev} size="large" sx={{ p: '0 20px' }}>
                <ArrowBack />
            </IconButton>
            <Box mx={2} textAlign="center">
                <Typography variant="h6" sx={{ letterSpacing: '0.02857em', textTransform: 'uppercase' }}>
                    {label}
                </Typography>
            </Box>
            <IconButton onClick={handleNext} size="large" sx={{ p: '0 20px' }}>
                <ArrowForward />
            </IconButton>
        </Box >
    );
}

export default YagoCardSContentSelection